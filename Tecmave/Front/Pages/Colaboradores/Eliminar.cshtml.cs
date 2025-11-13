using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Front.Data;
using Front.Models;

namespace Front.Pages.Colaboradores
{
    public class EliminarModel : PageModel
    {
        private readonly MyIdentityDBContext _context;

        [BindProperty]
        public Colaborador Colaborador { get; set; }

        public EliminarModel(MyIdentityDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Colaborador = await _context.Colaboradores.FindAsync(id);

            if (Colaborador == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);

            if (colaborador == null)
                return NotFound();

            _context.Colaboradores.Remove(colaborador);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Colaborador eliminado correctamente.";
            return RedirectToPage("Index");
        }
    }
}
