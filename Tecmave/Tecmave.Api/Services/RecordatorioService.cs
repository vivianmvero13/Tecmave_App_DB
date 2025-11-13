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
            while (!stoppingToken.IsCancellationRequested)
            {
                await EnviarRecordatorios(stoppingToken);
                await Task.Delay(TimeSpan.FromHours(30), stoppingToken);
            }
        }

        private async Task EnviarRecordatorios(CancellationToken ct)
        {

            _logger.LogInformation("Prueba de la comprobación de los recordatorios");
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            _logger.LogInformation("El contexto si se cargó");

           
            var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
            var notificacionesService = scope.ServiceProvider.GetRequiredService<NotificacionesService>();
            var SeisMeses = DateOnly.FromDateTime(DateTime.Today.AddMonths(-6));

            var mantenimientos = await context.agendamientos
                .GroupBy(a => a.vehiculo_id)
                .Select(g => g.OrderByDescending(a => a.fecha_agregada).FirstOrDefault())
                .ToListAsync(ct);

            _logger.LogInformation("Se encontró {count} registros de mantenimiento", mantenimientos.Count);
            foreach (var m in mantenimientos)
            {
                if (m.fecha_agregada <= SeisMeses)
                {
                    var vehiculo = await context.Vehiculos.FirstOrDefaultAsync(v => v.IdVehiculo == m.vehiculo_id, ct);
                    if (vehiculo == null)
                    {
                        continue;
                    }
                    if (vehiculo.Proximo.HasValue)
                    {
                        var usuario = await context.usuarios.FirstOrDefaultAsync(u => u.Id == m.cliente_id && u.NotificacionesActivadas, ct);
                        if (usuario == null)
                        {
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
                            _logger.LogError(ex, "Error al enviar recordatorio a {Email} para vehículo {Placa}", usuario.Email, vehiculo.Placa);

                        }
                    }
                }
            }

        }
    }
}
