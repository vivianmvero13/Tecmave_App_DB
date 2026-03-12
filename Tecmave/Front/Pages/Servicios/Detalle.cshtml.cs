using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Front.Pages.Servicios
{
    [Authorize]
    public class DetalleModel : PageModel
    {
        public int RevisionId { get; set; }

        public void OnGet(int id)
        {
            RevisionId = id;
        }
    }
}
