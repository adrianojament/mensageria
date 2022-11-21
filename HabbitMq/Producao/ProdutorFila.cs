using Dominio.Model;
using HabbitMq.Configuracoes;
using HabbitMq.Consumo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HabbitMq.Producao
{
    public class ProdutorFila : IProdutorFila
    {
        private Configuracao _HabbitMQConfiguracao;
        private readonly ILogger<ConsumidorFila> _logger;
        private readonly ConnectionFactory _Factory;

        public ProdutorFila(IOptions<Configuracao> option, ILogger<ConsumidorFila> logger)
        {
            _HabbitMQConfiguracao = option.Value;
            _logger = logger;
            _Factory = new ConnectionFactory
            {
                HostName = _HabbitMQConfiguracao.Host
            };
        }

        public Task ExecutarAsync(MensagemModel mensagem)
        {
            using (var connection = _Factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: _HabbitMQConfiguracao.Queue,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var stringfiedMessage = JsonSerializer.Serialize(mensagem);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: _HabbitMQConfiguracao.Queue,
                        basicProperties: null,
                        body: bytesMessage);
                }
            }

            return Task.CompletedTask;
        }
    }
}
