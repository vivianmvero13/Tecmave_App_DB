using Microsoft.AspNetCore.Identity;

namespace Tecmave.Api.Models
{
    public class AppRole : IdentityRole<int>
    {
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
