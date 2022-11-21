using HabbitMq.Consumo;

namespace Consumidor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConsumidorFila _ConsumidorFila;

        public Worker(ILogger<Worker> logger
            , IConsumidorFila consumidorFila)
        {
            _logger = logger;

            _ConsumidorFila = consumidorFila;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _ConsumidorFila.ExecutarAsync();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}