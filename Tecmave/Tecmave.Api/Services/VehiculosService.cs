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

        private static string NormalizarPlaca(string? placaSinNormalizar)
        {
            return (placaSinNormalizar ?? string.Empty)
                .ToUpperInvariant()
                .Trim();
        }

        // Listar todos
        public async Task<List<Vehiculo>> ListAsync(CancellationToken ct = default)
        {
            return await _context.Vehiculos
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Obtener por id
        public async Task<Vehiculo?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Vehiculos
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.IdVehiculo == id, ct);
        }

        // Crear
        public async Task<Vehiculo> AddAsync(Vehiculo v, CancellationToken ct = default)
        {
            v.Placa = NormalizarPlaca(v.Placa);

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
            v.Placa = NormalizarPlaca(input.Placa);
            v.Modelo = (input.Modelo ?? string.Empty).Trim();

            await _context.SaveChangesAsync(ct);
            return true;
        }

        // Eliminar (retorna false si no existe)
        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var v = await _context.Vehiculos
                .FirstOrDefaultAsync(x => x.IdVehiculo == id, ct);

            if (v is null) return false;

            // Cargar y eliminar agendamientos asociados
            // Ajusta "vehiculo_id" según el nombre real de tu propiedad FK.
            var agendamientos = await _context.agendamientos
                .Where(a => a.vehiculo_id == id)
                .ToListAsync(ct);

            using var tx = await _context.Database.BeginTransactionAsync(ct);

            try
            {
                if (agendamientos.Any())
                {
                    _context.agendamientos.RemoveRange(agendamientos);
                }

                _context.Vehiculos.Remove(v);

                await _context.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);

                return true;
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }
    }
}
