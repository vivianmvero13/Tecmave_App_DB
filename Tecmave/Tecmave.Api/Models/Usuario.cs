// Tecmave.Api/Models/Usuario.cs  (o en tu lib compartida)
using Microsoft.AspNetCore.Identity;

namespace Tecmave.Api.Models
{
    public class Usuario : IdentityUser<int>
    {
        public string Nombre { get; set; } = "";
        public string Apellidos { get; set; } = "";
        public string Cedula { get; set; } = "";
        public string Direccion { get; set; } = "";
    }
}
