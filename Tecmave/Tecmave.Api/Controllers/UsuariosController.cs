using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly UserAdminService _svc;
        public UsuariosController(UserAdminService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> List() => Ok(await _svc.ListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
            => (await _svc.GetByIdAsync(id)) is var u && u is not null ? Ok(u) : NotFound();

        public record CreateUserDto(string UserName, string Email, string Password, string? PhoneNumber);
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var (res, user) = await _svc.CreateAsync(dto.UserName, dto.Email, dto.Password, dto.PhoneNumber);
            return res.Succeeded ? CreatedAtAction(nameof(Get), new { id = user!.Id }, user) : BadRequest(res.Errors);
        }

        public record UpdateUserDto(string? UserName, string? Email, string? PhoneNumber);
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            var res = await _svc.UpdateAsync(id, dto.UserName, dto.Email, dto.PhoneNumber);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _svc.DeleteAsync(id);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }

        [HttpGet("{id:int}/roles")]
        public async Task<IActionResult> GetRoles(int id) => Ok(await _svc.GetRolesAsync(id));

        [HttpGet("{id:int}/role")]
        public async Task<IActionResult> GetSingleRole(int id)
        {
            var r = await _svc.GetSingleRoleOrNullAsync(id);
            return Ok(new { role = r });
        }

        public record AssignRoleDto(string RoleName, bool ForceReplace = false);

        [HttpPut("{id:int}/role")]
        public async Task<IActionResult> SetSingleRole(int id, [FromBody] AssignRoleDto dto, CancellationToken ct)
        {
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
        public async Task<IActionResult> RemoveAllRoles(int id, CancellationToken ct)
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
