using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Front.Models;
using Front.Data;

namespace Front.Pages.Colaboradores
{
    public class CrearModel : PageModel
    {
        private readonly MyIdentityDBContext _context;

        [BindProperty]
        public Colaborador Colaborador { get; set; }

        public CrearModel(MyIdentityDBContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Colaboradores.Add(Colaborador);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Colaborador creado correctamente.";
            return RedirectToPage("Index");
        }
    }
}
