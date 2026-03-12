using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Front.Pages.Mantenimientos
{
    [Authorize(Roles = "Admin,Colaborador")]
    public class EditarModel : PageModel
    {
        [FromRoute]
        public int Id { get; set; }

        public void OnGet()
        {
            // El JS se encarga de llamar a la API /Agendamiento/{id}
        }
    }
}
