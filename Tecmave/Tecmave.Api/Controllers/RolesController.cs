using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly RolesService _svc;
        public RolesController(RolesService svc) { _svc = svc; }

        public record CreateRoleDto(string Name, string? Description, bool IsActive = true);
        public record UpdateRoleDto(string? Name, string? Description, bool? IsActive);
        public record PermissionDto(string Permission);

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var roles = await _svc.ListAsync();
            return Ok(roles.Select(r => new
            {
                r.Id,
                r.Name,
                r.NormalizedName,
                r.Description,
                r.IsActive
            }));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var r = await _svc.FindByIdAsync(id);
            return r is null ? NotFound() : Ok(new
            {
                r.Id,
                r.Name,
                r.NormalizedName,
                r.Description,
                r.IsActive
            });
        }

        [HttpGet("{id:int}/details")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var r = await _svc.FindByIdAsync(id);
            if (r is null) return NotFound();

            var perms = await _svc.GetPermissionsAsync(id);
            return Ok(new
            {
                r.Id,
                r.Name,
                r.Description,
                r.IsActive,
                Permissions = perms
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            var (res, role) = await _svc.CreateAsync(dto.Name, dto.Description, dto.IsActive);
            if (!res.Succeeded) return BadRequest(res.Errors);

            return CreatedAtAction(nameof(Get), new { id = role!.Id }, new
            {
                role.Id,
                role.Name,
                role.NormalizedName,
                role.Description,
                role.IsActive
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleDto dto)
        {
            var res = await _svc.UpdateAsync(id, dto.Name, dto.Description, dto.IsActive);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }

        [HttpPost("{id:int}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var res = await _svc.SetActiveAsync(id, true);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }

        [HttpPost("{id:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var res = await _svc.SetActiveAsync(id, false);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _svc.DeleteAsync(id);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }

        [HttpGet("{id:int}/permissions")]
        public async Task<IActionResult> GetPermissions(int id)
        {
            var list = await _svc.GetPermissionsAsync(id);
            return Ok(list);
        }

        [HttpPost("{id:int}/permissions")]
        public async Task<IActionResult> AddPermission(int id, [FromBody] PermissionDto dto)
        {
            var res = await _svc.AddPermissionAsync(id, dto.Permission);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }

        [HttpDelete("{id:int}/permissions")]
        public async Task<IActionResult> RemovePermission(int id, [FromQuery] string permission)
        {
            var res = await _svc.RemovePermissionAsync(id, permission);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }
    }
}
