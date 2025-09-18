using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AlphatechFront.Models
{
    // TAREA PARA COMPAÑEROS:
    // Este es el modelo de Usuario base. Si necesitan más campos
    // (como Teléfono, Dirección, etc.), deben agregarlos aquí.
    public class Usuario : IdentityUser
    {
        
        public string NombreCompleto { get; set; }



    }
}
