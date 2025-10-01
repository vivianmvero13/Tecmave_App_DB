using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class RolesService
    {
        private readonly RoleManager<AppRole> _roles;

        public RolesService(RoleManager<AppRole> roles)
        {
            _roles = roles;
        }

        public Task<List<AppRole>> GetAllAsync() =>
            _roles.Roles.AsNoTracking().ToListAsync();

        public Task<AppRole?> GetByIdAsync(int id) =>
            _roles.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);

        public async Task<IdentityResult> CreateAsync(string name, string? description = null, bool isActive = true)
        {
            var role = new AppRole
            {
                Name = name,
                NormalizedName = name.ToUpperInvariant(),
                Description = description,
                IsActive = isActive
            };
            return await _roles.CreateAsync(role);
        }

        public async Task<IdentityResult> UpdateAsync(int id, string? name = null, string? description = null, bool? isActive = null)
        {
            var role = await _roles.FindByIdAsync(id.ToString());
            if (role is null) return IdentityResult.Failed(new IdentityError { Description = "Rol no encontrado" });

            if (!string.IsNullOrWhiteSpace(name))
            {
                role.Name = name;
                role.NormalizedName = name.ToUpperInvariant();
            }
            if (description is not null) role.Description = description;
            if (isActive is not null) role.IsActive = isActive.Value;

            return await _roles.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteAsync(int id)
        {
            var role = await _roles.FindByIdAsync(id.ToString());
            if (role is null) return IdentityResult.Failed(new IdentityError { Description = "Rol no encontrado" });
            return await _roles.DeleteAsync(role);
        }
    }
}
