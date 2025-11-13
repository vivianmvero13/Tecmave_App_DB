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

        // DETALLE: también incluye Vehículo + Cliente (por si lo usas en la página Detalle)
        public MantenimientoModel? GetById(int id) =>
            _context.Mantenimientos
                .Include(m => m.Vehiculo)
                    .ThenInclude(v => v.Cliente)
                .FirstOrDefault(m => m.IdMantenimiento == id);

        public MantenimientoModel AddMantenimiento(MantenimientoModel mantenimiento)
        {
            // 1) Si el admin NO seteó PróximoMantenimiento, lo calculamos (historial)
            if (!mantenimiento.ProximoMantenimiento.HasValue)
            {
                mantenimiento.ProximoMantenimiento = mantenimiento.FechaMantenimiento.AddMonths(6);
            }

            // 2) Antes de agregar el nuevo, marcamos los anteriores de ese vehículo
            var anteriores = _context.Mantenimientos
                .Where(m => m.IdVehiculo == mantenimiento.IdVehiculo && !m.RecordatorioEnviado)
                .ToList();

            foreach (var a in anteriores)
            {
                a.RecordatorioEnviado = true;
            }

            // 3) Ahora sí agregamos el nuevo
            _context.Mantenimientos.Add(mantenimiento);
            _context.SaveChanges();

            return mantenimiento;
        }

        // RECORDATORIO INDIVIDUAL (botón por fila)
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
