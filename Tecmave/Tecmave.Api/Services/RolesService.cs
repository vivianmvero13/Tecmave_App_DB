using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Tecmave.Api.Services
{
    public class RolesService
    {
        private readonly RoleManager<IdentityRole<int>> _roles;

        public RolesService(RoleManager<IdentityRole<int>> roles)
        {
            _roles = roles;
        }

        public Task<List<IdentityRole<int>>> GetAllAsync() =>
            _roles.Roles.AsNoTracking().ToListAsync();

        public Task<IdentityResult> CreateAsync(string name) =>
            _roles.CreateAsync(new IdentityRole<int>(name));

        public async Task<IdentityResult> DeleteAsync(int id)
        {
            var role = await _roles.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role == null) return IdentityResult.Failed(new IdentityError { Description = "Role not found" });
            return await _roles.DeleteAsync(role);
        }
    }
}
