using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class MantenimientoService
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public MantenimientoService(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public List<MantenimientoModel> GetMantenimientoModels() =>
            _context.Mantenimientos.ToList();

        public MantenimientoModel? GetById(int id) =>
            _context.Mantenimientos.FirstOrDefault(m => m.IdMantenimiento == id);

        public MantenimientoModel AddMantenimiento(MantenimientoModel mantenimiento)
        {
            _context.Mantenimientos.Add(mantenimiento);

            var anterior = _context.Mantenimientos
                .Where(m => m.IdVehiculo == mantenimiento.IdVehiculo && !m.RecordatorioEnviado)
                .ToList();

            foreach (var a in anterior)
            {
                a.RecordatorioEnviado = true;
            }

            _context.SaveChanges();
            return mantenimiento;
        }

        public bool UpdateMantenimiento(MantenimientoModel mantenimiento)
        {
            if (!_context.Mantenimientos.Any(m => m.IdMantenimiento == mantenimiento.IdMantenimiento))
                return false;

            _context.Mantenimientos.Update(mantenimiento);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteMantenimiento(int id)
        {
            var mantenimiento = _context.Mantenimientos.FirstOrDefault(m => m.IdMantenimiento == id);
            if (mantenimiento == null)
                return false;
            _context.Mantenimientos.Remove(mantenimiento);
            _context.SaveChanges();
            return true;
        }

        public async Task EnviarRecordatorioAsync()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            var vencido = await _context.Mantenimientos
                .Include(m => m.Vehiculo)
                .ThenInclude(v => v.Cliente)
                .Where(m => !m.RecordatorioEnviado &&
                            EF.Functions.DateDiffMonth(m.FechaMantenimiento, hoy) >= 6)
                .ToListAsync();

            foreach (var v in vencido)
            {
                await _emailService.EnviarCorreo(
                    v.Vehiculo.Cliente.Email,
                    "Recordatorio de Mantenimiento",
                    $"Ya han pasado 6 meses desde el último mantenimiento de su vehículo {v.Vehiculo.Placa}. "
                );

                v.RecordatorioEnviado = true;
            }

            await _context.SaveChangesAsync();
        }


    }
}
