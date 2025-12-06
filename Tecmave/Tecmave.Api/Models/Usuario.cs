using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class Usuario : IdentityUser<int>
    {
        [MaxLength(50)]
        public string? Nombre { get; set; }

        [MaxLength(50)]
        public string? Apellido { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}".Trim();

        public bool NotificacionesActivadas { get; set; } = false;
        public int Estado { get; set; } = 1;

        public string Cedula { get; set; }

        public string Direccion { get; set; }
    }
}
