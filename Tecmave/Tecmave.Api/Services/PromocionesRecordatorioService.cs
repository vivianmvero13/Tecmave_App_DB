using Tecmave.Api.Services;

public class PromocionesRecordatorioService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PromocionesRecordatorioService> _logger;

    public PromocionesRecordatorioService(
        IServiceScopeFactory scopeFactory,
        ILogger<PromocionesRecordatorioService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PromocionesRecordatorioService iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var promosService = scope.ServiceProvider.GetRequiredService<PromocionesService>();

                await promosService.EnviarRecordatoriosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar EnviarRecordatoriosAsync de promociones");
            }

            // Idealmente una vez al día
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
