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


            if (res.Succeeded)
            {
                
                var roleExists = await _roleManager.RoleExistsAsync("Cliente");
                if (!roleExists)
                {
                    var clienteRole = new IdentityRole<int>
                    {
                        Name = "Cliente",
                        NormalizedName = "CLIENTE"
                    };

                    await _roleManager.CreateAsync(clienteRole);
                }

                
                await _userManager.AddToRoleAsync(user, "Cliente");
            }
            else
            {
                // Manejo de errores si la creación falla
                throw new Exception("Error al crear el usuario: " + string.Join(", ", res.Errors.Select(e => e.Description)));
            }


            if (ReturnUrl == null)
            {
                ReturnUrl = "/";
            }

            return LocalRedirect(ReturnUrl);
        }
    }
}
