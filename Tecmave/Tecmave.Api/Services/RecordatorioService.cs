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
            using var scope = _scopeFactory.CreateScope();

            var mantenimientoService = scope.ServiceProvider.GetRequiredService<MantenimientoService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<RecordatorioService>>();

            logger.LogInformation("Ejecutando envío automático de recordatorios de mantenimiento...");

            await mantenimientoService.EnviarRecordatorioAsync();

            logger.LogInformation("Finalizó el envío automático de recordatorios.");
        }

    }
}
