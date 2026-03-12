using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Front.Pages.Planillas
{
    [Authorize(Roles = "Admin")]
    public class CrearModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
