using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Models;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole<int>> _roles;
        private readonly UserManager<Usuario> _users;

        public RolesController(RoleManager<IdentityRole<int>> roles, UserManager<Usuario> users)
        {
            _roles = roles;
            _users = users;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityRole<int>>>> GetAll()
        {
            var list = await _roles.Roles.AsNoTracking().ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<IdentityRole<int>>> GetById(int id)
        {
            var role = await _roles.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        public record CreateRoleDto(string Name);
        [HttpPost]
        public async Task<ActionResult<IdentityRole<int>>> Create([FromBody] CreateRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Name is required.");
            var exists = await _roles.RoleExistsAsync(dto.Name);
            if (exists) return Conflict("Role already exists.");
            var result = await _roles.CreateAsync(new IdentityRole<int>(dto.Name));
            if (!result.Succeeded) return BadRequest(result.Errors.Select(e => e.Description));
            var created = await _roles.Roles.AsNoTracking().FirstAsync(r => r.Name == dto.Name);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        public record UpdateRoleDto(string Name);
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Rename(int id, [FromBody] UpdateRoleDto dto)
        {
            var role = await _roles.FindByIdAsync(id.ToString());
            if (role == null) return NotFound();
            if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Name is required.");
            var dup = await _roles.Roles.AnyAsync(r => r.Name == dto.Name && r.Id != id);
            if (dup) return Conflict("Another role with that name already exists.");
            var set = await _roles.SetRoleNameAsync(role, dto.Name);
            if (!set.Succeeded) return BadRequest(set.Errors.Select(e => e.Description));
            var upd = await _roles.UpdateAsync(role);
            if (!upd.Succeeded) return BadRequest(upd.Errors.Select(e => e.Description));
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roles.FindByIdAsync(id.ToString());
            if (role == null) return NotFound();
            var result = await _roles.DeleteAsync(role);
            if (!result.Succeeded) return BadRequest(result.Errors.Select(e => e.Description));
            return NoContent();
        }

        [HttpPost("{id:int}/users/{userId:int}")]
        public async Task<IActionResult> AddUserToRole(int id, int userId)
        {
            var role = await _roles.FindByIdAsync(id.ToString());
            if (role == null) return NotFound("Role not found.");
            var user = await _users.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound("User not found.");
            var result = await _users.AddToRoleAsync(user, role.Name!);
            if (!result.Succeeded) return BadRequest(result.Errors.Select(e => e.Description));
            return NoContent();
        }

        [HttpDelete("{id:int}/users/{userId:int}")]
        public async Task<IActionResult> RemoveUserFromRole(int id, int userId)
        {
            var role = await _roles.FindByIdAsync(id.ToString());
            if (role == null) return NotFound("Role not found.");
            var user = await _users.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound("User not found.");
            var result = await _users.RemoveFromRoleAsync(user, role.Name!);
            if (!result.Succeeded) return BadRequest(result.Errors.Select(e => e.Description));
            return NoContent();
        }
    }
}
