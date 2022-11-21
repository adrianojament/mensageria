using HabbitMq.Configuracoes;
using HabbitMq.Producao;
using Produtor;
using Produtor.Aplicacao;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<Configuracao>(hostContext.Configuration.GetSection("RabbitMqConfig"));
        services.AddSingleton<IProdutorFila, ProdutorFila>();
        services.AddSingleton<IPublicacaoAplicacao, PublicacaoAplicacao>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
