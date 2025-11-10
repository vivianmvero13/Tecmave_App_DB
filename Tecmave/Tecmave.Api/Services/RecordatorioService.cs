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
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task EnviarRecordatorios(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
            var notificacionesService = scope.ServiceProvider.GetRequiredService<NotificacionesService>();
            var fechaObjectivo = DateTime.Today.AddDays(3);

            var promociones = await context.promociones
                .Where(p => p.fecha_inicio == DateOnly.FromDateTime(fechaObjectivo))
                .ToListAsync(ct);

            if (!promociones.Any())
            {
                _logger.LogInformation("No hay promociones para enviar recordatorios.");
                return;
            }

            var usuarios = await context.usuarios
                .Where(u => u.NotificacionesActivadas)
                .ToListAsync(ct);

            if (!usuarios.Any())
            {
                _logger.LogInformation("No hay usuarios con notificaciones activadas.");
                return;
            }

            foreach (var promo in promociones)
            {
                foreach (var usuario in usuarios)
                {
                    var asunto = "Recordatorio de Promoción Próxima: {promo.Titulo}";
                    var cuerpo = $@"
                        <h1>Hola {usuario.NombreCompleto},</h1>
                        <p>Te recordamos que la promoción <strong>{promo.titulo}</strong> comienza el {promo.fecha_inicio:dd/MM/yyyy}.</p>
                        <p>{promo.descripcion}</p>
                        <p>¡No te la pierdas!</p>";

                    try
                    {
                        await notificacionesService.CrearNotificacionAsync(usuario.Id, $"Recordatorio: {promo.titulo}", "Recordatorio");
                        await emailService.EnviarCorreo(usuario.Email, asunto, cuerpo);
                        _logger.LogInformation("Recordatorio enviado a {Email} para la promoción {PromoTitulo}.", usuario.Email, promo.titulo);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al enviar recordatorio a {Email} para la promoción {PromoTitulo}.", usuario.Email, promo.titulo);
                    }
                }
            }
        }
    }
}
