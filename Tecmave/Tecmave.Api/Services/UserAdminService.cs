using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class UserAdminService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly AppDbContext _db;

        public UserAdminService(UserManager<Usuario> um, RoleManager<AppRole> rm, AppDbContext db)
        {
            _userManager = um;
            _roleManager = rm;
            _db = db;
        }

        public Task<List<Usuario>> ListAsync() =>
            _userManager.Users.AsNoTracking().OrderBy(u => u.UserName).ToListAsync();

        public Task<Usuario?> GetByIdAsync(int id) =>
            _userManager.FindByIdAsync(id.ToString());

        public async Task<(IdentityResult result, Usuario? user)> CreateAsync(string userName, string email, string password, string? phone = null)
        {
            var user = new Usuario { UserName = userName, Email = email, PhoneNumber = phone };
            var res = await _userManager.CreateAsync(user, password);
            return (res, res.Succeeded ? user : null);
        }

        public async Task EnsureRoleAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new AppRole { Name = roleName, NormalizedName = roleName.ToUpperInvariant(), IsActive = true });
        }

        public async Task<IList<string>> GetRolesAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return user is null ? new List<string>() : await _userManager.GetRolesAsync(user);
        }

        public async Task<string?> GetSingleRoleOrNullAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) return null;
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }

        public async Task<(IdentityResult result, string? previousRole)> SetSingleRoleAsync(
            int userId, string roleName, bool forceReplace,
            int? adminId = null, string? adminName = null, string? ip = null)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return (IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = "Usuario no encontrado" }), null);

            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.NormalizedName == roleName.ToUpper());
            if (role is null)
                return (IdentityResult.Failed(new IdentityError { Code = "RoleNotFound", Description = "Rol no encontrado" }), null);

            if (!role.IsActive)
                return (IdentityResult.Failed(new IdentityError { Code = "RoleInactive", Description = "El rol está inactivo" }), null);

            var currentRoles = await _userManager.GetRolesAsync(user);
            var previous = currentRoles.FirstOrDefault();

            if (previous != null && previous.Equals(role.Name, StringComparison.OrdinalIgnoreCase))
                return (IdentityResult.Success, previous);

            if (currentRoles.Count > 0 && !forceReplace)
                return (IdentityResult.Failed(new IdentityError { Code = "RoleConflict", Description = $"El usuario ya tiene el rol '{previous}'" }), previous);

            if (currentRoles.Count > 0)
            {
                var rem = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!rem.Succeeded) return (rem, previous);
            }

            await EnsureRoleAsync(role.Name!);
            var addRes = await _userManager.AddToRoleAsync(user, role.Name!);

            if (addRes.Succeeded)
            {
                _db.role_change_audit.Add(new RoleChangeAudit
                {
                    TargetUserId = user.Id,
                    TargetUserName = user.UserName,
                    PreviousRole = previous,
                    NewRole = role.Name,
                    ChangedByUserId = adminId,
                    ChangedByUserName = adminName,
                    ChangedAtUtc = DateTime.UtcNow,
                    Action = previous == null ? "Asignar" : "Reemplazar",
                    Detail = previous == null ? $"Asignación de rol '{role.Name}'" : $"Reemplazo de rol '{previous}' por '{role.Name}'",
                    SourceIp = ip
                });
                await _db.SaveChangesAsync();
            }

            return (addRes, previous);
        }

        public async Task<IdentityResult> RemoveAllRolesAsync(
            int userId, int? adminId = null, string? adminName = null, string? ip = null)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = "Usuario no encontrado" });

            var current = await _userManager.GetRolesAsync(user);
            if (current.Count == 0) return IdentityResult.Success;

            var res = await _userManager.RemoveFromRolesAsync(user, current);
            if (res.Succeeded)
            {
                _db.role_change_audit.Add(new RoleChangeAudit
                {
                    TargetUserId = user.Id,
                    TargetUserName = user.UserName,
                    PreviousRole = current.FirstOrDefault(),
                    NewRole = null,
                    ChangedByUserId = adminId,
                    ChangedByUserName = adminName,
                    ChangedAtUtc = DateTime.UtcNow,
                    Action = "Eliminar",
                    Detail = "Eliminación de rol",
                    SourceIp = ip
                });
                await _db.SaveChangesAsync();
            }
            return res;
        }

        public async Task<IdentityResult> UpdateAsync(int id, string? userName = null, string? email = null, string? phone = null)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "Usuario no encontrado" });

            if (!string.IsNullOrWhiteSpace(userName))
                user.UserName = userName;
            if (!string.IsNullOrWhiteSpace(email))
                user.Email = email;
            if (!string.IsNullOrWhiteSpace(phone))
                user.PhoneNumber = phone;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "Usuario no encontrado" });
            return await _userManager.DeleteAsync(user);
        }
    }
}
