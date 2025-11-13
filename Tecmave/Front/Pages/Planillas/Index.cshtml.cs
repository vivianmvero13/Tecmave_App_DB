using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Front.Data;
using Front.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Front.Pages.Planillas
{
    public class IndexModel : PageModel
    {
        private readonly MyIdentityDBContext _context;
        public IndexModel(MyIdentityDBContext context) { _context = context; }
        public List<Planilla> Planillas { get; set; } = new();
        public SelectList ColaboradoresSelect { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? FiltroColaboradorId { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? FiltroInicio { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? FiltroFin { get; set; }
        public async Task OnGetAsync()
        {
            var colabs = await _context.Colaboradores.OrderBy(c => c.Nombre).ToListAsync();
            ColaboradoresSelect = new SelectList(colabs, "Id", "Nombre");
            var q = _context.Planillas.Include(p => p.Colaborador).AsQueryable();
            if (FiltroColaboradorId.HasValue) q = q.Where(p => p.ColaboradorId == FiltroColaboradorId.Value);
            if (FiltroInicio.HasValue) q = q.Where(p => p.PeriodoInicio >= FiltroInicio.Value);
            if (FiltroFin.HasValue) q = q.Where(p => p.PeriodoFin <= FiltroFin.Value);
            Planillas = await q.OrderByDescending(p => p.FechaGenerada).ToListAsync();
        }
    }
}
