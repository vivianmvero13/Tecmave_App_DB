using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Front.Pages.Factura
{
    [Authorize(Roles = "Admin,Colaborador")]
    public class MisFacturasModel : PageModel
    {
        public bool IsCliente => User.IsInRole("Cliente");
        public int? ClienteId { get; private set; }

        public void OnGet()
        {
            // 1) Identity normalmente guarda el Id en NameIdentifier
            var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 2) Si tu Id es int:
            if (int.TryParse(raw, out var id))
                ClienteId = id;
        }
    }
}
