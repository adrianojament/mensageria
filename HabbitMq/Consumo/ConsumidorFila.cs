using Dominio.Model;
using HabbitMq.Configuracoes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.Common;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace HabbitMq.Consumo
{
    public class ConsumidorFila : IConsumidorFila
    {
        private Configuracao _HabbitMQConfiguracao;
        private readonly ILogger<ConsumidorFila> _logger;
        private readonly ConnectionFactory _Factory;

        public ConsumidorFila(IOptions<Configuracao> option, ILogger<ConsumidorFila> logger)
        {
            _HabbitMQConfiguracao = option.Value;
            _logger = logger;

            _Factory = new ConnectionFactory
            {
                HostName = _HabbitMQConfiguracao.Host
            };

        }

        public Task ExecutarAsync()
        {
            using (var connection = _Factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var _connection = _Factory.CreateConnection();
                    var _channel = _connection.CreateModel();
                    _channel.QueueDeclare(
                                queue: _HabbitMQConfiguracao.Queue,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
                    var consumer = new EventingBasicConsumer(_channel);

                    consumer.Received += (sender, eventArgs) =>
                    {
                        var contentArray = eventArgs.Body.ToArray();
                        var contentString = Encoding.UTF8.GetString(contentArray);
                        var message = JsonSerializer.Deserialize<MensagemModel>(contentString);
                        
                        if (message != null)
                            Notificar(message);
                        
                        _channel.BasicAck(eventArgs.DeliveryTag, false);
                    };

                    _channel.BasicConsume(_HabbitMQConfiguracao.Queue, false, consumer);
                }
            }

            return Task.CompletedTask;
        }

        private void Notificar(MensagemModel mensagem)
        {
            _logger.LogInformation($"Origem - {mensagem.Origem}" +
                $" Mensagem: {mensagem.Conteudo}" +
                $" Data Criação: {mensagem.CriadoEm}");
        }
    }
}
