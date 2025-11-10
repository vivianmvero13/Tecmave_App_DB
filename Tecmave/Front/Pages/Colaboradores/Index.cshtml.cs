using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Front.Data;
using Front.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Front.Pages.Colaboradores
{
    public class IndexModel : PageModel
    {
        private readonly MyIdentityDBContext _context;

        public IndexModel(MyIdentityDBContext context)
        {
            _context = context;
        }

        public IList<Colaborador> Colaboradores { get; set; } = new List<Colaborador>();

        public async Task OnGetAsync()
        {
            Colaboradores = await _context.Set<Colaborador>().ToListAsync();
        }
    }
}
