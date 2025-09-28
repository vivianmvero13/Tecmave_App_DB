using Microsoft.AspNetCore.Identity;

namespace Tecmave.Api.Models
{
    public class RolesModel : IdentityRole<int>
    {
        public string? Descripcion { get; set; }
    }
}
