using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class RolesService
    {
        private readonly RoleManager<RolesModel> _roleManager;

        public RolesService(RoleManager<RolesModel> roleManager)
        {
            _roleManager = roleManager;
        }

        public Task<List<RolesModel>> GetAllAsync() =>
            _roleManager.Roles.AsNoTracking().ToListAsync();

        public Task<RolesModel?> GetByIdAsync(int id) =>
            _roleManager.FindByIdAsync(id.ToString());

        public async Task<IdentityResult> CreateAsync(string name, string? descripcion = null)
        {
            if (await _roleManager.RoleExistsAsync(name))
                return IdentityResult.Failed(new IdentityError { Description = "El rol ya existe." });

            var role = new RolesModel
            {
                Name = name,
                NormalizedName = name.ToUpperInvariant(),
                Descripcion = descripcion
            };
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> UpdateAsync(int id, string? name = null, string? descripcion = null)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
                return IdentityResult.Failed(new IdentityError { Description = "Rol no encontrado." });

            if (!string.IsNullOrWhiteSpace(name))
            {
                role.Name = name;
                role.NormalizedName = name.ToUpperInvariant();
            }
            if (descripcion != null)
                role.Descripcion = descripcion;

            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteAsync(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
                return IdentityResult.Failed(new IdentityError { Description = "Rol no encontrado." });

            return await _roleManager.DeleteAsync(role);
        }
    }
}
