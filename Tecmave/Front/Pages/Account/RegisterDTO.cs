using System.ComponentModel.DataAnnotations;

namespace Front.Pages.Account
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Debe confirmar su contraseña.")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string Password2 { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [RegularExpression("^[1-9]\\d{8}$", ErrorMessage = "La cédula debe ser de 9 dígitos.")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [RegularExpression("^[246789]\\d{7}$", ErrorMessage = "Debe ser un teléfono válido de Costa Rica.")]
        public string PhoneNumber { get; set; }

        public bool NotificacionesActivadas { get; set; } = false;
    }
}
