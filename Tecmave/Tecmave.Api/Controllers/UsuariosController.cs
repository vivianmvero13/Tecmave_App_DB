using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    // [Authorize] // habilita según tus políticas
    public class UsuariosController : ControllerBase
    {
        private readonly UserAdminService _svc;
        public UsuariosController(UserAdminService svc) => _svc = svc;

        // DTOs salida: no exponer campos sensibles
        public record UsuarioItemDto(int Id, string? Nombre, string? Apellido, string UserName, string? Email, string? PhoneNumber);

        // DTOs entrada
        public record CreateUserDto(
            [Required, StringLength(50)] string nombre,
            [Required, StringLength(50)] string apellido,
            [Required, StringLength(256)] string UserName,
            [Required, EmailAddress] string Email,
            [Required, MinLength(6)] string Password,
            string? PhoneNumber);

        public record UpdateUserDto(string? nombre, string? apellido, string? UserName, string? Email, string? PhoneNumber);
        public record AssignRoleDto([Required] string RoleName, bool ForceReplace = false);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioItemDto>>> List()
        {
            var users = await _svc.ListAsync();
            var data = users.Select(u => new UsuarioItemDto(u.Id, u.Nombre, u.Apellido, u.UserName!, u.Email, u.PhoneNumber));
            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var u = await _svc.GetByIdAsync(id);
            if (u is null) return NotFound();
            return Ok(new UsuarioItemDto(u.Id, u.Nombre, u.Apellido, u.UserName!, u.Email, u.PhoneNumber));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var (res, user) = await _svc.CreateAsync(dto.nombre, dto.apellido, dto.UserName, dto.Email, dto.Password, dto.PhoneNumber);
            if (!res.Succeeded) return BadRequest(res.Errors);
            var outDto = new UsuarioItemDto(user!.Id, user.Nombre, user.Apellido, user.UserName!, user.Email, user.PhoneNumber);
            return CreatedAtAction(nameof(Get), new { id = user!.Id }, outDto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var res = await _svc.UpdateAsync(id, dto.nombre, dto.apellido, dto.UserName, dto.Email, dto.PhoneNumber);
            return res.Succeeded ? NoContent() : BadRequest(res.Errors);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _svc.DeleteAsync(id);
            return res.Succeeded ? NoContent() : BadRequest(res.Errors);
        }

        [HttpGet("{id:int}/roles")]
        public async Task<IActionResult> GetRoles(int id) => Ok(await _svc.GetRolesAsync(id));

        [HttpPut("{id:int}/role")]
        public async Task<IActionResult> SetSingleRole(int id, [FromBody] AssignRoleDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var adminIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? adminId = int.TryParse(adminIdStr, out var n) ? n : null;
            var adminName = User.Identity?.Name;
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            var (res, previous) = await _svc.SetSingleRoleAsync(id, dto.RoleName, dto.ForceReplace, adminId, adminName, ip);
            if (!res.Succeeded)
            {
                var err = res.Errors.FirstOrDefault();
                var code = err?.Code ?? "Error";
                var msg = err?.Description ?? "Error";
                return code switch
                {
                    "UserNotFound" => NotFound(new { code, message = msg }),
                    "RoleNotFound" => NotFound(new { code, message = msg }),
                    "RoleInactive" => BadRequest(new { code, message = msg }),
                    "RoleConflict" => Conflict(new { code, message = msg, previous }),
                    _ => BadRequest(new { code, message = msg })
                };
            }
            return Ok(new { previous, current = dto.RoleName });
        }

        [HttpDelete("{id:int}/role")]
        public async Task<IActionResult> RemoveAllRoles(int id)
        {
            var adminIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? adminId = int.TryParse(adminIdStr, out var n) ? n : null;
            var adminName = User.Identity?.Name;
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            var res = await _svc.RemoveAllRolesAsync(id, adminId, adminName, ip);
            return res.Succeeded ? Ok(new { role = (string?)null }) : BadRequest(res.Errors);
        }
    }
}
