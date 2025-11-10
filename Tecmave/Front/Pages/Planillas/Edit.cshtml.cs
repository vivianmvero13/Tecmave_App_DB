using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Front.Data;
using Front.Models;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Front.Pages.Planillas
{
    public class EditModel : PageModel
    {
        private readonly MyIdentityDBContext _context;
        public EditModel(MyIdentityDBContext context) { _context = context; }
        [BindProperty]
        public Planilla Planilla { get; set; }
        public SelectList ColaboradoresSelect { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Planilla = await _context.Planillas.FindAsync(id);
            if (Planilla == null) return NotFound();
            var colabs = _context.Colaboradores.OrderBy(c => c.Nombre).ToList();
            ColaboradoresSelect = new SelectList(colabs, "Id", "Nombre", Planilla.ColaboradorId);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            Planilla.TotalSalario = Math.Round(Planilla.HorasTrabajadas * Planilla.ValorHora, 2);
            Planilla.NetoPagar = Math.Round(Planilla.TotalSalario - Planilla.Deducciones, 2);
            _context.Planillas.Update(Planilla);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
