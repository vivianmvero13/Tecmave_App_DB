using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
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

        // -----------------------
        // DTOs salida
        // -----------------------
        public record UsuarioItemDto(
            int Id,
            string? Nombre,
            string? Apellidos,
            string UserName,
            string? Email,
            string? PhoneNumber,
            string Cedula,
            int Estado
        );

        // -----------------------
        // DTOs entrada
        // -----------------------
        public record CreateUserDto(
            [Required, StringLength(50)] string nombre,
            [Required, StringLength(50)] string apellidos,
            [Required, StringLength(256)] string UserName,
            [Required, EmailAddress] string Email,
            [Required, MinLength(6)] string Password,
            string? PhoneNumber,
            string? Cedula
        );

        public record UpdateUserDto(
            string? nombre,
            string? apellidos,
            string? UserName,
            string? Email,
            string? PhoneNumber,
            string? Cedula
        );

        public record AssignRoleDto([Required] string RoleName, bool ForceReplace = false);

        // -----------------------
        // GET: api/usuarios
        // -----------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioItemDto>>> List()
        {
            var users = await _svc.ListAsync();

            var data = users.Select(u => new UsuarioItemDto(
                u.Id,
                u.Nombre,
                u.Apellido,
                u.UserName!,
                u.Email,
                u.PhoneNumber,
                u.Cedula ?? string.Empty,
                u.Estado
            ));

            return Ok(data);
        }

        // -----------------------
        // GET: api/usuarios/{id}
        // -----------------------
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var u = await _svc.GetByIdAsync(id);
            if (u is null) return NotFound();

            return Ok(new UsuarioItemDto(
                u.Id,
                u.Nombre,
                u.Apellido,
                u.UserName!,
                u.Email,
                u.PhoneNumber,
                u.Cedula ?? string.Empty,
                u.Estado
            ));
        }

        // -----------------------
        // POST: api/usuarios
        // -----------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var (res, user) = await _svc.CreateAsync(
                dto.nombre,
                dto.apellidos,
                dto.UserName,
                dto.Email,
                dto.Password,
                dto.PhoneNumber,
                dto.Cedula
            );

            if (!res.Succeeded) return BadRequest(res.Errors);

            var outDto = new UsuarioItemDto(
                user!.Id,
                user.Nombre,
                user.Apellido,
                user.UserName!,
                user.Email,
                user.PhoneNumber,
                user.Cedula ?? string.Empty,
                user.Estado
            );

            return CreatedAtAction(nameof(Get), new { id = user.Id }, outDto);
        }

        // -----------------------
        // PUT: api/usuarios/{id}
        // -----------------------
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // Si tu service aún no actualiza cédula, podés:
            // 1) agregar lógica en el service UpdateAsync
            // 2) o dejarlo así y solo exponerlo para más adelante
            var res = await _svc.UpdateAsync(
                id,
                dto.nombre,
                dto.apellidos,
                dto.UserName,
                dto.Email,
                dto.PhoneNumber
            );

            // Nota: aquí NO se actualiza Cedula todavía porque tu UpdateAsync no la recibe.
            // Si querés que se actualice, te paso el cambio del service también.

            return res.Succeeded ? NoContent() : BadRequest(res.Errors);
        }

        // -----------------------
        // DELETE: api/usuarios/{id}
        // -----------------------
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _svc.DeleteAsync(id);
            return res.Succeeded ? NoContent() : BadRequest(res.Errors);
        }

        // -----------------------
        // ROLES
        // -----------------------
        [HttpGet("{id:int}/roles")]
        public async Task<IActionResult> GetRoles(int id) => Ok(await _svc.GetRolesAsync(id));

        public record SingleRoleDto(string? Role);

        [HttpGet("{id:int}/role")]
        public async Task<IActionResult> GetSingleRole(int id)
        {
            var role = await _svc.GetSingleRoleOrNullAsync(id);
            return Ok(new SingleRoleDto(role));
        }

        [HttpPut("{id:int}/role")]
        public async Task<IActionResult> SetSingleRole(int id, [FromBody] AssignRoleDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var adminIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? adminId = int.TryParse(adminIdStr, out var n) ? n : null;
            var adminName = User.Identity?.Name;
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            var (res, previous) = await _svc.SetSingleRoleAsync(
                id,
                dto.RoleName,
                dto.ForceReplace,
                adminId,
                adminName,
                ip
            );

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
