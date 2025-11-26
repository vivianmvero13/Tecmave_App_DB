using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text;
using Tecmave.Front.Models;

namespace Front.Pages.Account
{


    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;

        public ResetPasswordModel(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Token { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }

        public string Message { get; set; }

        public void OnGet(string email, string token)
        {
            Email = email;

           

            Token = token;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(NewPassword))
            {
                ModelState.AddModelError(string.Empty, "Debe ingresar una nueva contraseña.");
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                Message = "Error: usuario no encontrado.";
                return Page();
            }

            var result = await _userManager.ResetPasswordAsync(user, Token, NewPassword);
            if (result.Succeeded)
            {
                return Redirect("/Account/Login?msg=ok");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }
    }


}
