using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text;
using Tecmave.Api.Services;
using Tecmave.Front.Models;

namespace Front.Pages.Account
{

    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<Usuario> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public string Email { get; set; }

        public string Message { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                ModelState.AddModelError(string.Empty, "Debe ingresar un correo válido.");
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                // No revelar si existe
                Message = "Si el correo existe, se enviará un enlace para restablecer la contraseña.";
                return Page();
            }

            // Generar token y codificarlo para URL
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
       

            var resetLink = Url.Page("/Account/ResetPassword", null,
                new { email = Email, token = token }, Request.Scheme);

            var bodyHtml = $@"
                <h2>Recuperación de contraseña</h2>
                <p>Haz clic en el siguiente botón para restablecer tu contraseña:</p>
                <p>
                    <a href=""{resetLink}"" 
                    style=""background:#007bff;color:white;padding:12px 20px;border-radius:8px;
                           text-decoration:none;font-weight:bold;"">
                        Restablecer contraseña
                    </a>
                </p>";

            await _emailSender.SendAsync(Email, "Recuperar contraseña", bodyHtml);

            Message = "Si el correo existe, se enviará un enlace para restablecer la contraseña.";
            return Page();
        }
    }

}
