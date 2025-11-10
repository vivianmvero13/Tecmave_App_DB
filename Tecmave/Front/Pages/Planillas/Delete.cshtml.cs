using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Front.Data;
using Front.Models;
using System.Threading.Tasks;

namespace Front.Pages.Planillas
{
    public class DeleteModel : PageModel
    {
        private readonly MyIdentityDBContext _context;
        public DeleteModel(MyIdentityDBContext context) { _context = context; }
        [BindProperty]
        public Planilla Planilla { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Planilla = await _context.Planillas.FindAsync(id);
            if (Planilla == null) return NotFound();
            Planilla.Colaborador = await _context.Colaboradores.FindAsync(Planilla.ColaboradorId);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var entidad = await _context.Planillas.FindAsync(id);
            if (entidad == null) return NotFound();
            _context.Planillas.Remove(entidad);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
