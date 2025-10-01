using System.ComponentModel.DataAnnotations;

namespace Tecmave.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Error en los digitos")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        [Compare("ConfirmNewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nueva contraseña")]
        public string ConfirmNewPassword { get; set; }
    }
}
