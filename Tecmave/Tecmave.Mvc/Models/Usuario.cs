using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Tecmave.Models
{

    public class Usuario : IdentityUser
    {
        
        public string NombreCompleto { get; set; }

        public string Cedula { get; set; }

    }
}
