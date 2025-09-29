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

        [HttpGet] public async Task<IActionResult> List() => Ok(await _svc.ListAsync());

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

        [HttpPost("{id:int}/roles/add")]
        public async Task<IActionResult> AddRole(int id, [FromQuery] string role)
        {
            var res = await _svc.AddToRoleAsync(id, role);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }

        [HttpPost("{id:int}/roles/remove")]
        public async Task<IActionResult> RemoveRole(int id, [FromQuery] string role)
        {
            var res = await _svc.RemoveFromRoleAsync(id, role);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }

        [HttpGet("{id:int}/roles")]
        public async Task<IActionResult> GetRoles(int id)
            => Ok(await _svc.GetRolesAsync(id));
    }
}
