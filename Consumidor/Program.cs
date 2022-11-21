using Consumidor;
using HabbitMq.Configuracoes;
using HabbitMq.Consumo;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<Configuracao>(hostContext.Configuration.GetSection("RabbitMqConfig"));
        services.AddSingleton<IConsumidorFila, ConsumidorFila>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
