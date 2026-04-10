using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Front.Pages.Revisiones
{
    [Authorize(Roles = "Colaborador,Admin")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
