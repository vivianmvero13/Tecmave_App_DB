using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class UserAdminService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<RolesModel> _roleManager;

        public UserAdminService(UserManager<Usuario> um, RoleManager<RolesModel> rm)
        {
            _userManager = um; _roleManager = rm;
        }

        public Task<List<Usuario>> ListAsync() =>
            _userManager.Users.AsNoTracking().ToListAsync();

        public Task<Usuario?> GetByIdAsync(int id) =>
            _userManager.FindByIdAsync(id.ToString());

        public async Task<(IdentityResult result, Usuario? user)> CreateAsync(string userName, string email, string password, string? phone = null)
        {
            var user = new Usuario { UserName = userName, Email = email, PhoneNumber = phone };
            var res = await _userManager.CreateAsync(user, password);
            return (res, res.Succeeded ? user : null);
        }

        public async Task<IdentityResult> UpdateAsync(int id, string? userName = null, string? email = null, string? phone = null)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) return IdentityResult.Failed(new IdentityError { Description = "Usuario no encontrado" });
            if (!string.IsNullOrWhiteSpace(userName)) user.UserName = userName;
            if (!string.IsNullOrWhiteSpace(email)) user.Email = email;
            if (!string.IsNullOrWhiteSpace(phone)) user.PhoneNumber = phone;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) return IdentityResult.Failed(new IdentityError { Description = "Usuario no encontrado" });
            return await _userManager.DeleteAsync(user);
        }

        public Task<string?> GeneratePasswordResetTokenAsync(int id) =>
            GetByIdAsync(id).ContinueWith(async t => t.Result is null ? null : await _userManager.GeneratePasswordResetTokenAsync(t.Result)).Unwrap();

        public async Task<IdentityResult> ResetPasswordWithTokenAsync(int id, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) return IdentityResult.Failed(new IdentityError { Description = "Usuario no encontrado" });
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task EnsureRoleAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new RolesModel { Name = roleName, NormalizedName = roleName.ToUpperInvariant() });
        }

        public async Task<IdentityResult> AddToRoleAsync(int id, string roleName)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) return IdentityResult.Failed(new IdentityError { Description = "Usuario no encontrado" });
            await EnsureRoleAsync(roleName);
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(int id, string roleName)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) return IdentityResult.Failed(new IdentityError { Description = "Usuario no encontrado" });
            return await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task<IList<string>> GetRolesAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return user is null ? new List<string>() : await _userManager.GetRolesAsync(user);
        }
    }
}
