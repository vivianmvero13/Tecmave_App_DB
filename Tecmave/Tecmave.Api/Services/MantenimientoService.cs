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

        // LISTADO: incluye Vehículo + Cliente (para el front)
        public List<MantenimientoModel> GetMantenimientoModels() =>
            _context.Mantenimientos
                .Include(m => m.Vehiculo)
                    .ThenInclude(v => v.Cliente)
                .ToList();

        // DETALLE: también incluye Vehículo + Cliente
        public MantenimientoModel? GetById(int id) =>
            _context.Mantenimientos
                .Include(m => m.Vehiculo)
                    .ThenInclude(v => v.Cliente)
                .FirstOrDefault(m => m.IdMantenimiento == id);

        public MantenimientoModel AddMantenimiento(MantenimientoModel mantenimiento)
        {
            if (mantenimiento.IdEstado == 0)
            {
                mantenimiento.IdEstado = 12;
            }

            if (!mantenimiento.ProximoMantenimiento.HasValue)
            {
                mantenimiento.ProximoMantenimiento = mantenimiento.FechaMantenimiento.AddMonths(6);
            }

            var anteriores = _context.Mantenimientos
                .Where(m => m.IdVehiculo == mantenimiento.IdVehiculo && !m.RecordatorioEnviado)
                .ToList();

            foreach (var a in anteriores)
            {
                a.RecordatorioEnviado = true;
            }

            _context.Mantenimientos.Add(mantenimiento);
            _context.SaveChanges();

            return mantenimiento;
        }

        // RECORDATORIO INDIVIDUAL
        public async Task<bool> EnviarRecordatorioIndividualAsync(int idMantenimiento)
        {
            var m = await _context.Mantenimientos
                .Include(x => x.Vehiculo)
                    .ThenInclude(v => v.Cliente)
                .FirstOrDefaultAsync(x => x.IdMantenimiento == idMantenimiento);

            if (m == null || m.Vehiculo == null || m.Vehiculo.Cliente == null)
                return false;

            if (!m.ProximoMantenimiento.HasValue)
                return false;

            // Evitar reenviar si ya fue enviado
            if (m.RecordatorioEnviado)
                return true; // o false, según tu criterio de UX

            var asunto = $"Recordatorio de mantenimiento para su vehículo {m.Vehiculo.Placa}";
            var cuerpo = $@"
<p>Estimado/a {m.Vehiculo.Cliente.Nombre},</p>
<p>Le recordamos que se aproxima la fecha de mantenimiento de su vehículo
con placa <strong>{m.Vehiculo.Placa}</strong>.</p>
<p>Próxima fecha estimada: <strong>{m.ProximoMantenimiento:dd/MM/yyyy}</strong>.</p>
<p>Por favor contáctenos para agendar su cita.</p>
<p>Atentamente,<br/>El equipo de Tecmave</p>";

            await _emailService.EnviarCorreo(m.Vehiculo.Cliente.Email, asunto, cuerpo);

            m.RecordatorioEnviado = true;
            await _context.SaveChangesAsync();

            return true;
        }

        //Actualizar estado del mantenimiento
        public async Task<(bool ok, string mensaje)> ActualizarEstadoAsync(int idMantenimiento, int idEstado)
        {
            try
            {
                var mantenimiento = await _context.Mantenimientos
                    .Include(m => m.Vehiculo)
                        .ThenInclude(v => v.Cliente)
                    .FirstOrDefaultAsync(m => m.IdMantenimiento == idMantenimiento);

                if (mantenimiento == null)
                    return (false, "Mantenimiento no encontrado");

                mantenimiento.IdEstado = idEstado;

                await _context.SaveChangesAsync();
                return (true, "Estado actualizado correctamente");
            }
            catch
            {
                return (false, "Error al actualizar estado del servicio");
            }
        }

        public bool UpdateMantenimiento(MantenimientoModel mantenimiento)
        {
            var existente = _context.Mantenimientos
                .FirstOrDefault(m => m.IdMantenimiento == mantenimiento.IdMantenimiento);

            if (existente == null)
                return false;

            existente.FechaMantenimiento = mantenimiento.FechaMantenimiento;

            if (mantenimiento.ProximoMantenimiento.HasValue)
            {
                existente.ProximoMantenimiento = mantenimiento.ProximoMantenimiento;
                existente.RecordatorioEnviado = false; // reprogramar recordatorio
            }

            _context.SaveChanges();
            return true;
        }

        public bool DeleteMantenimiento(int id)
        {
            var mantenimiento = _context.Mantenimientos
                .FirstOrDefault(m => m.IdMantenimiento == id);

            if (mantenimiento == null)
                return false;

            _context.Mantenimientos.Remove(mantenimiento);
            _context.SaveChanges();
            return true;
        }

        // RECORDATORIOS AUTOMÁTICOS (para el BackgroundService o botón masivo)
        public async Task EnviarRecordatorioAsync()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var limite = hoy.AddDays(3);

            var pendientes = await _context.Mantenimientos
                .Include(m => m.Vehiculo)
                    .ThenInclude(v => v.Cliente)
                .Where(m => !m.RecordatorioEnviado &&
                            m.ProximoMantenimiento.HasValue &&
                            m.ProximoMantenimiento.Value >= hoy &&
                            m.ProximoMantenimiento.Value <= limite)
                .ToListAsync();

            foreach (var m in pendientes)
            {
                var asunto = $"Recordatorio de mantenimiento para su vehículo {m.Vehiculo.Placa}";
                var cuerpo = $@"
<p>Estimado/a {m.Vehiculo.Cliente.Nombre},</p>
<p>Le recordamos que se aproxima la fecha de mantenimiento de su vehículo
con placa <strong>{m.Vehiculo.Placa}</strong>.</p>
<p>Próxima fecha estimada: <strong>{m.ProximoMantenimiento:dd/MM/yyyy}</strong>.</p>
<p>Por favor contáctenos para agendar su cita.</p>
<p>Atentamente,<br/>El equipo de Tecmave</p>";

                await _emailService.EnviarCorreo(
                    m.Vehiculo.Cliente.Email,
                    asunto,
                    cuerpo
                );

                m.RecordatorioEnviado = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}