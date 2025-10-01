using System.ComponentModel.DataAnnotations;

namespace Tecmave.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        public string NombreCompleto { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Cedula { get; set; }
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Error en los digitos")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage ="Las contraseñas no coinciden.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
