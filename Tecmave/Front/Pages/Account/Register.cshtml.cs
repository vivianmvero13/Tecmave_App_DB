using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tecmave.Front.Models;

namespace Front.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;

        private readonly RoleManager<IdentityRole<int>> _roleManager;


        [BindProperty]
        public RegisterDTO Register { get; set; }
        public string ReturnUrl { get; set; }

        public RegisterModel(UserManager<Usuario> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Register.Password != Register.Password2)
            {
                throw new Exception("Passwords no coinciden");
            }

            var existingUser = await _userManager.FindByEmailAsync(Register.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Register.Email",
                    "Este correo ya está registrado. Intenta iniciar sesión o usar otro correo.");
                return Page();
            }

            var user = new Usuario();
            user.Email = Register.Email;
            user.UserName = Register.Email;
            user.Cedula = Register.Cedula;
            user.Nombre = Register.Nombre;
            user.Apellido = Register.Apellido;
            user.Direccion = Register.Direccion;
            user.PhoneNumber = Register.PhoneNumber;
            user.NotificacionesActivadas = Register.NotificacionesActivadas;

            var res = await _userManager
                .CreateAsync(user, Register.Password);


            if (!res.Succeeded)
            {
                // Agregar errores de Identity al ModelState (password débil, etc.)
                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            // Verificar/crear rol Cliente
            if (!await _roleManager.RoleExistsAsync("Cliente"))
            {
                await _roleManager.CreateAsync(new IdentityRole<int>
                {
                    Name = "Cliente",
                    NormalizedName = "CLIENTE"
                });
            }

            await _userManager.AddToRoleAsync(user, "Cliente");



            return LocalRedirect(ReturnUrl ?? "/");
        }
    }
}
