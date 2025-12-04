using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Front.Pages.Usuarios
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguration _cfg;
        public EditarModel(IConfiguration cfg) { _cfg = cfg; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public string ApiBase { get; private set; } = "";

        public void OnGet()
        {
            ApiBase = _cfg.GetSection("Api")["BaseUrl"] ?? "https://tecmave-api.azurewebsites.net";
        }
    }
}
