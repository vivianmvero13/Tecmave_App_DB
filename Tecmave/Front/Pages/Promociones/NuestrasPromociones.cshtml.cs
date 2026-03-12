using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Front.Pages.Promociones
{
    [Authorize(Roles = "Admin,Colaborador")]
    public class NuestrasPromocionesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
