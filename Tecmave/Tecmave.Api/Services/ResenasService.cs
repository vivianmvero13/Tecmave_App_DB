using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class ResenasService
    {
        private readonly AppDbContext _context;
        public ResenasService(AppDbContext context) => _context = context;

        public Task<List<ResenasModel>> GetResenasAsync()
            => _context.resenas
                .OrderByDescending(r => r.fecha)
                .ToListAsync();

        public Task<ResenasModel?> GetByIdAsync(int id)
            => _context.resenas.FirstOrDefaultAsync(p => p.id_resena == id);

        public async Task<ResenasModel> AddAsync(ResenasModel item)
        {
            _context.resenas.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> UpdateComentarioAsync(int id, string comentario)
        {
            var entidad = await _context.resenas.FirstOrDefaultAsync(p => p.id_resena == id);
            if (entidad == null) return false;

            entidad.comentario = comentario;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entidad = await _context.resenas.FirstOrDefaultAsync(p => p.id_resena == id);
            if (entidad == null) return false;

            _context.resenas.Remove(entidad);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
