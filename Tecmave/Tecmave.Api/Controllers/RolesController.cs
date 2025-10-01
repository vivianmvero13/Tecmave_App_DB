using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly RolesService _svc;
        public RolesController(RolesService svc) { _svc = svc; }

        // DTOs
        public record CreateRoleDto(string Name, string? Description, bool IsActive = true);
        public record UpdateRoleDto(string? Name, string? Description, bool? IsActive);
        public record PermissionDto(string Permission);

        // GET /api/roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> List()
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

        // GET /api/roles/5
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

        // POST /api/roles
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            var (res, role) = await _svc.CreateAsync(dto.Name, dto.Description, dto.IsActive);
            return res.Succeeded
                ? CreatedAtAction(nameof(Get), new { id = role!.Id }, new
                {
                    role.Id,
                    role.Name,
                    role.NormalizedName,
                    role.Description,
                    role.IsActive
                })
                : BadRequest(res.Errors);
        }

        // PUT /api/roles/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleDto dto)
        {
            var res = await _svc.UpdateAsync(id, dto.Name, dto.Description, dto.IsActive);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }

        // POST /api/roles/5/activate
        [HttpPost("{id:int}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var res = await _svc.SetActiveAsync(id, true);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }

        // POST /api/roles/5/deactivate
        [HttpPost("{id:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var res = await _svc.SetActiveAsync(id, false);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }

        // DELETE /api/roles/5  (protege si hay usuarios asignados)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _svc.DeleteAsync(id);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }

        // --- Permisos (role claims) ---

        // GET /api/roles/5/permissions
        [HttpGet("{id:int}/permissions")]
        public async Task<IActionResult> GetPermissions(int id)
        {
            var list = await _svc.GetPermissionsAsync(id);
            return Ok(list);
        }

        // POST /api/roles/5/permissions
        [HttpPost("{id:int}/permissions")]
        public async Task<IActionResult> AddPermission(int id, [FromBody] PermissionDto dto)
        {
            var res = await _svc.AddPermissionAsync(id, dto.Permission);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }

        // DELETE /api/roles/5/permissions?permission=xyz
        [HttpDelete("{id:int}/permissions")]
        public async Task<IActionResult> RemovePermission(int id, [FromQuery] string permission)
        {
            var res = await _svc.RemovePermissionAsync(id, permission);
            return res.Succeeded ? Ok() : BadRequest(res.Errors);
        }
    }
}
