using System.Security.Claims;
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

        // -----------------------
        // Lectura
        // -----------------------
        public Task<List<Usuario>> ListAsync() =>
            _userManager.Users
                .AsNoTracking()
                .OrderBy(u => u.UserName)
                .ToListAsync();

        public Task<Usuario?> GetByIdAsync(int id) =>
            _userManager.FindByIdAsync(id.ToString());

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

        // -----------------------
        // Creación
        // -----------------------
        public async Task<(IdentityResult result, Usuario? user)> CreateAsync(
            string nombre,
            string apellidos,
            string userName,
            string email,
            string password,
            string? phone = null)
        {
            var user = new Usuario
            {
                Nombre = nombre,
                Apellidos = apellidos,
                UserName = userName,
                Email = email,
                PhoneNumber = phone
            };

            var res = await _userManager.CreateAsync(user, password);
            if (!res.Succeeded) return (res, null);

            // Claims de nombre (opcional, útil para mostrar en UI)
            await EnsureNameClaimsAsync(user);

            return (IdentityResult.Success, user);
        }

        // -----------------------
        // Roles
        // -----------------------
        public async Task EnsureRoleAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new AppRole
                {
                    Name = roleName,
                    // NormalizedName lo maneja RoleManager internamente
                    IsActive = true
                };
                await _roleManager.CreateAsync(role);
            }
        }

        public async Task<(IdentityResult result, string? previousRole)> SetSingleRoleAsync(
            int userId,
            string roleName,
            bool forceReplace,
            int? adminId = null,
            string? adminName = null,
            string? ip = null)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return (IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = "Usuario no encontrado" }), null);

            var normalized = roleName.ToUpperInvariant();
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalized);
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

        // -----------------------
        // Actualización
        // -----------------------
        public async Task<IdentityResult> UpdateAsync(
            int id,
            string? nombre = null,
            string? apellidos = null,
            string? userName = null,
            string? email = null,
            string? phone = null)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "Usuario no encontrado" });

            var errors = new List<IdentityError>();

            // Campos personalizados (se asignan directo)
            var changedNames = false;
            if (nombre != null && nombre != user.Nombre)
            {
                user.Nombre = nombre;
                changedNames = true;
            }
            if (apellidos != null && apellidos != user.Apellidos)
            {
                user.Apellidos = apellidos;
                changedNames = true;
            }

            // UserName (normaliza/valida)
            if (!string.IsNullOrWhiteSpace(userName) && userName != user.UserName)
            {
                var r = await _userManager.SetUserNameAsync(user, userName);
                if (!r.Succeeded) errors.AddRange(r.Errors);
            }

            // Email (normaliza/valida)
            if (!string.IsNullOrWhiteSpace(email) && email != user.Email)
            {
                var r = await _userManager.SetEmailAsync(user, email);
                if (!r.Succeeded) errors.AddRange(r.Errors);
            }

            // Teléfono
            if (phone != null && phone != user.PhoneNumber)
            {
                var r = await _userManager.SetPhoneNumberAsync(user, phone);
                if (!r.Succeeded) errors.AddRange(r.Errors);
            }

            // Guarda cambios (incluye props custom)
            var up = await _userManager.UpdateAsync(user);
            if (!up.Succeeded) errors.AddRange(up.Errors);

            if (errors.Count > 0)
                return IdentityResult.Failed(errors.ToArray());

            // Refresca claims de nombre si cambió
            if (changedNames)
                await EnsureNameClaimsAsync(user);

            return IdentityResult.Success;
        }

        // -----------------------
        // Eliminación
        // -----------------------
        public async Task<IdentityResult> DeleteAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "Usuario no encontrado" });

            return await _userManager.DeleteAsync(user);
        }

        // -----------------------
        // Helpers
        // -----------------------
        private async Task EnsureNameClaimsAsync(Usuario user)
        {
            var claims = await _userManager.GetClaimsAsync(user);

            var given = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            var sur = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);
            var full = claims.FirstOrDefault(c => c.Type == "full_name");

            if (given != null) await _userManager.RemoveClaimAsync(user, given);
            if (sur != null) await _userManager.RemoveClaimAsync(user, sur);
            if (full != null) await _userManager.RemoveClaimAsync(user, full);

            if (!string.IsNullOrWhiteSpace(user.Nombre))
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, user.Nombre));
            if (!string.IsNullOrWhiteSpace(user.Apellidos))
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Surname, user.Apellidos));

            var nombreCompleto = $"{user.Nombre} {user.Apellidos}".Trim();
            await _userManager.AddClaimAsync(user, new Claim("full_name", nombreCompleto));
        }
    }
}
