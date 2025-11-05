using Microsoft.AspNetCore.Identity;

namespace Tecmave.Front.Models
{
    public class Usuario : IdentityUser<int>
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Cedula { get; set; }
        public string Direccion { get; set; }
    }
}