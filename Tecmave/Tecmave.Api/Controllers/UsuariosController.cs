using Microsoft.AspNetCore.Mvc;
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


        [HttpGet("{id:int}/role")]
        public async Task<IActionResult> GetSingleRole(int id)
        {
            var role = await _svc.GetSingleRoleOrNullAsync(id);
            return Ok(new { Role = role });
        }

        public record AssignRoleDto(string RoleName, bool ForceReplace = false);

        [HttpPut("{id:int}/role")]
        public async Task<IActionResult> SetSingleRole(int id, [FromBody] AssignRoleDto dto)
        {
            var (res, previous) = await _svc.SetSingleRoleAsync(id, dto.RoleName, dto.ForceReplace);
            if (res.Succeeded) return Ok(new { Previous = previous, Current = dto.RoleName });

            var code = res.Errors.FirstOrDefault()?.Code ?? "";
            var desc = res.Errors.FirstOrDefault()?.Description ?? "Error";

            return code switch
            {
                "UserNotFound" => NotFound(new { Code = code, Message = desc }),
                "RoleNotFound" => NotFound(new { Code = code, Message = desc }),
                "RoleInactive" => BadRequest(new { Code = code, Message = desc }),
                "RoleConflict" => Conflict(new { Code = code, Message = desc, Previous = previous }),
                _ => BadRequest(new { Code = code, Message = desc })
            };
        }

        [HttpDelete("{id:int}/role")]
        public async Task<IActionResult> RemoveAllRoles(int id)
        {
            var res = await _svc.RemoveAllRolesAsync(id);
            if (res.Succeeded) return Ok(new { Role = (string?)null });
            var err = res.Errors.FirstOrDefault();
            if (err?.Code == "UserNotFound") return NotFound(err);
            return BadRequest(res.Errors);
        }
    }
}
