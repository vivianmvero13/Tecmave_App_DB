using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Front.Pages.Planillas
{
    [Authorize(Roles = "Admin")]
    public class EditarModel : PageModel
    {
        [BindProperty]
        public int Id { get; set; }

        public void OnGet(int id)
        {
            Id = id;
        }
    }
}
