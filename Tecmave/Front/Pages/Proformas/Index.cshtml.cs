using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Front.Pages.Proformas
{
    [Authorize(Roles = "Admin,Colaborador")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
