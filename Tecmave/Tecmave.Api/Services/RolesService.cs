using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class RolesService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RolesService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public Task<List<AppRole>> ListAsync() =>
            _roleManager.Roles.AsNoTracking().OrderBy(r => r.Name).ToListAsync();

        public Task<AppRole?> FindByIdAsync(int id) =>
            _roleManager.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);

        public Task<AppRole?> FindByNameAsync(string roleName) =>
            _roleManager.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.NormalizedName == roleName.ToUpper());

        public async Task<(IdentityResult result, AppRole? role)> CreateAsync(string name, string? description, bool isActive)
        {
            var role = new AppRole
            {
                Name = name,
                NormalizedName = name?.ToUpperInvariant(),
                Description = description,
                IsActive = isActive
            };
            var res = await _roleManager.CreateAsync(role);
            return (res, res.Succeeded ? role : null);
        }

        public async Task<IdentityResult> UpdateAsync(int id, string? name, string? description, bool? isActive)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
                return IdentityResult.Failed(new IdentityError { Description = "Rol no encontrado" });

            if (!string.IsNullOrWhiteSpace(name))
            {
                role.Name = name;
                role.NormalizedName = name.ToUpperInvariant();
            }
            if (description is not null) role.Description = description;
            if (isActive is not null) role.IsActive = isActive.Value;

            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> SetActiveAsync(int id, bool isActive)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
                return IdentityResult.Failed(new IdentityError { Description = "Rol no encontrado" });

            role.IsActive = isActive;
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteAsync(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
                return IdentityResult.Failed(new IdentityError { Description = "Rol no encontrado" });

            return await _roleManager.DeleteAsync(role);
        }

        public async Task<List<string>> GetPermissionsAsync(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null) return new List<string>();

            var claims = await _roleManager.GetClaimsAsync(role);
            return claims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value)
                .Distinct()
                .ToList();
        }

        public async Task<IdentityResult> AddPermissionAsync(int id, string permission)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
                return IdentityResult.Failed(new IdentityError { Description = "Rol no encontrado" });

            var claims = await _roleManager.GetClaimsAsync(role);
            if (claims.Any(c => c.Type == "permission" && c.Value == permission))
                return IdentityResult.Success;

            return await _roleManager.AddClaimAsync(role, new Claim("permission", permission));
        }

        public async Task<IdentityResult> RemovePermissionAsync(int id, string permission)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
                return IdentityResult.Failed(new IdentityError { Description = "Rol no encontrado" });

            var claims = await _roleManager.GetClaimsAsync(role);
            var claim = claims.FirstOrDefault(c => c.Type == "permission" && c.Value == permission);
            if (claim is null) return IdentityResult.Success;

            return await _roleManager.RemoveClaimAsync(role, claim);
        }

        public async Task EnsureDefaultRolesAsync(params (string Name, string? Description)[] roles)
        {
            foreach (var r in roles)
            {
                if (!await _roleManager.RoleExistsAsync(r.Name))
                {
                    await _roleManager.CreateAsync(new AppRole
                    {
                        Name = r.Name,
                        NormalizedName = r.Name.ToUpperInvariant(),
                        Description = r.Description,
                        IsActive = true
                    });
                }
            }
        }
    }
}
