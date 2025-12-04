using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Tecmave.Front.Services;

namespace Front.Pages.Contacto
{
    public class IndexModel : PageModel
    {
        private readonly ITecmaveEmailSender _emailSender;

        public IndexModel(ITecmaveEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [BindProperty]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(200, ErrorMessage = "El nombre es demasiado largo.")]
        public string Nombre { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Debe ingresar un correo válido.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Por favor, escriba su mensaje.")]
        [StringLength(4000, ErrorMessage = "El mensaje es demasiado largo (máx. 4000 caracteres).")]
        public string Mensaje { get; set; }

        public string Message { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var nombre = Nombre?.Trim();
            var email = Email?.Trim();
            var mensaje = Mensaje?.Trim();

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(mensaje))
            {
                ModelState.AddModelError(string.Empty, "Completa todos los campos.");
                return Page();
            }

            var subject = "Nuevo mensaje de contacto desde el sitio web";
            var bodyHtml = $@"
                <h2>Contacto desde el sitio</h2>

                <p><strong>Nombre:</strong> {System.Net.WebUtility.HtmlEncode(nombre)}</p>
                <p><strong>Email:</strong> {System.Net.WebUtility.HtmlEncode(email)}</p>

                <hr />

                <p><strong>Mensaje:</strong></p>

                <div style=""white-space:pre-wrap;line-height:1.5"">
                    {System.Net.WebUtility.HtmlEncode(mensaje)}
                </div>
            ";

            try
            {
                

                await _emailSender.SendAsync(Email, subject, bodyHtml);

                Message = "Mensaje enviado correctamente. Te contactaremos pronto.";

                ModelState.Clear();
                Nombre = Email = Mensaje = string.Empty;

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al enviar el mensaje: {ex.Message}");
                return Page();
            }
        }
    }
}
