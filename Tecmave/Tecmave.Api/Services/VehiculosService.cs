using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class VehiculosService
    {
        private readonly AppDbContext _context;

        public VehiculosService(AppDbContext context)
        {
            _context = context;
        }

        // Listar todos
        public async Task<List<Vehiculo>> ListAsync(CancellationToken ct = default)
            => await _context.Vehiculos.AsNoTracking().ToListAsync(ct);

        // Obtener por id
        public async Task<Vehiculo?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _context.Vehiculos.AsNoTracking()
                   .FirstOrDefaultAsync(v => v.IdVehiculo == id, ct);

        // Crear
        public async Task<Vehiculo> AddAsync(Vehiculo v, CancellationToken ct = default)
        {
            // Normaliza placa, valida mínimos si quieres
            v.Placa = (v.Placa ?? string.Empty).ToUpperInvariant();

            _context.Vehiculos.Add(v);
            await _context.SaveChangesAsync(ct);
            return v;
        }

        // Actualizar (retorna false si no existe)
        public async Task<bool> UpdateAsync(Vehiculo input, CancellationToken ct = default)
        {
            var v = await _context.Vehiculos
                                  .FirstOrDefaultAsync(x => x.IdVehiculo == input.IdVehiculo, ct);
            if (v is null) return false;

            v.ClienteId = input.ClienteId;
            v.IdMarca = input.IdMarca;
            v.Anno = input.Anno;
            v.Placa = (input.Placa ?? string.Empty).ToUpperInvariant();

            await _context.SaveChangesAsync(ct);
            return true;
        }

        // Eliminar (retorna false si no existe)
        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var v = await _context.Vehiculos.FirstOrDefaultAsync(x => x.IdVehiculo == id, ct);
            if (v is null) return false;

            _context.Vehiculos.Remove(v);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}
