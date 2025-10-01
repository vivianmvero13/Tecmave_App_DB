using System.ComponentModel.DataAnnotations;

namespace Tecmave.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
