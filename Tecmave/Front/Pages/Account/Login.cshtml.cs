using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tecmave.Front.Models;

namespace Front.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public LoginModel(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public LoginDTO Login { get; set; }

        public string ErrorMessage { get; set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByEmailAsync(Login.Email);
            if (user == null)
            {
                ErrorMessage = "Usuario y/o contrase�a son incorrectos";
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                Login.Password,
                isPersistent: true,
                lockoutOnFailure: false
            );
            if (!result.Succeeded)
            {
                ErrorMessage = "Usuario y/o contrase�a son inv�lidos";
                return Page();
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Administrador"))
                return RedirectToPage("/Vehiculos/Index");
            else
                return RedirectToPage("/Vehiculos/Index");
        }
    }

}
