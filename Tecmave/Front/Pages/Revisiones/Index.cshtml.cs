using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Front.Pages.Revisiones
{
    [Authorize(Roles = "Colaborador")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
