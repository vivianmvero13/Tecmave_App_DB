using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class RecordatorioService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RecordatorioService> _logger;

        public RecordatorioService(IServiceScopeFactory scopeFactory, ILogger<RecordatorioService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RecordatorioService iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await EnviarRecordatorios(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error no controlado en EnviarRecordatorios");
                }

                // PARA DESARROLLO puedes usar 1 minuto:
                // await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                await Task.Delay(TimeSpan.FromHours(30), stoppingToken);
            }
        }

        private async Task EnviarRecordatorios(CancellationToken ct)
        {
            _logger.LogInformation("Comenzando comprobación de recordatorios de mantenimiento...");

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
            var notificacionesService = scope.ServiceProvider.GetRequiredService<NotificacionesService>();

            _logger.LogInformation("AppDbContext y servicios cargados correctamente.");

            var seisMeses = DateOnly.FromDateTime(DateTime.Today.AddMonths(-6));

            var mantenimientos = await context.agendamientos
                .GroupBy(a => a.vehiculo_id)
                .Select(g => g.OrderByDescending(a => a.fecha_agregada).FirstOrDefault())
                .ToListAsync(ct);

            _logger.LogInformation("Se encontraron {count} registros de agendamiento (último por vehículo).", mantenimientos.Count);

            foreach (var m in mantenimientos)
            {
                if (m == null)
                    continue;

                if (m.fecha_agregada > seisMeses)
                    continue;

                var vehiculo = await context.Vehiculos
                    .FirstOrDefaultAsync(v => v.IdVehiculo == m.vehiculo_id, ct);

                if (vehiculo == null)
                {
                    _logger.LogWarning("No se encontró vehículo con Id {IdVehiculo}", m.vehiculo_id);
                    continue;
                }

                if (!vehiculo.Proximo.HasValue)
                {
                    _logger.LogInformation("Vehículo {Placa} no tiene próxima fecha definida, se omite.", vehiculo.Placa);
                    continue;
                }

                var usuario = await context.usuarios
                    .FirstOrDefaultAsync(u => u.Id == m.cliente_id && u.NotificacionesActivadas, ct);

                if (usuario == null)
                {
                    _logger.LogInformation(
                        "No se encontró usuario con Id {IdUsuario} o no tiene notificaciones activadas.",
                        m.cliente_id);
                    continue;
                }

                var asunto = $"Recordatorio de mantenimiento para su vehículo {vehiculo.Placa}";
                var cuerpo = $@"
<p>Estimado/a {usuario.Nombre},</p>
<p>Le recordamos que su vehículo con placa <strong>{vehiculo.Placa}</strong> no ha tenido un mantenimiento en los últimos seis meses.</p>
<p>Es importante realizar mantenimientos periódicos para asegurar el buen funcionamiento y la seguridad de su vehículo.</p>
<p>Por favor, póngase en contacto con nosotros para programar una cita de mantenimiento.</p>
<p>Atentamente,<br/>El equipo de Tecmave</p>";

                try
                {
                    await notificacionesService.CrearNotificacionAsync(
                        usuario.Id,
                        "Recordatorio de Mantenimiento",
                        "Es hora de realizar el mantenimiento de su vehículo."
                    );

                    await emailService.EnviarCorreo(usuario.Email, asunto, cuerpo);

                    context.recordatorios.Add(new Recordatorio
                    {
                        UsuarioId = usuario.Id,
                        VehiculoId = vehiculo.IdVehiculo,
                        FechaEnvio = DateTime.Now,
                        Tipo = "Semestre"
                    });

                    await context.SaveChangesAsync(ct);

                    _logger.LogInformation("Recordatorio enviado a {Email} para vehículo {Placa}", usuario.Email, vehiculo.Placa);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al procesar recordatorio para usuario {UsuarioId}, vehículo {VehiculoId}",
                        usuario.Id, vehiculo.IdVehiculo);
                }

            }

            _logger.LogInformation("Finalizó la comprobación de recordatorios de mantenimiento.");
        }
    }
}
