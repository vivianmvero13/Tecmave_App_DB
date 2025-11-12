using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Front.Pages.Usuarios
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _cfg;

        public IndexModel(IConfiguration cfg)
        {
            _cfg = cfg;
        }

        public string ApiBase { get; private set; } = "";

        public void OnGet()
        {
            ApiBase = _cfg.GetSection("Api")["BaseUrl"] ?? "https://localhost:7096";
        }
    }
}
