using Produtor.Aplicacao;

namespace Produtor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IPublicacaoAplicacao _PublicacaoAplicacao;

        public Worker(ILogger<Worker> logger, IPublicacaoAplicacao publicacaoAplicacao)
        {
            _logger = logger;
            _PublicacaoAplicacao = publicacaoAplicacao;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await _PublicacaoAplicacao.PublicarAsync();
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}