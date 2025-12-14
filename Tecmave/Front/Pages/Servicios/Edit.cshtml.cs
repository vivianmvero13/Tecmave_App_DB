using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Front.Pages.Servicios
{
    public class EditModel : PageModel
    {

        public int RevisionId { get; set; }

        public void OnGet(int id)
        {

            RevisionId = id;
        }
    }
}
