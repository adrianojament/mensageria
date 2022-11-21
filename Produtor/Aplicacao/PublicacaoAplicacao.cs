using Dominio.Model;
using HabbitMq.Configuracoes;
using HabbitMq.Consumo;
using HabbitMq.Producao;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Produtor.Aplicacao
{
    public class PublicacaoAplicacao : IPublicacaoAplicacao
    {
        private Configuracao _HabbitMQConfiguracao;
        private IProdutorFila _ProdutorFila;

        public PublicacaoAplicacao(IOptions<Configuracao> option
            , IProdutorFila produtorFila)
        {
            _HabbitMQConfiguracao = option.Value;
            _ProdutorFila = produtorFila;
        }

        public async Task PublicarAsync()
        {
            await _ProdutorFila.ExecutarAsync(new MensagemModel(_HabbitMQConfiguracao.Origin, "Hello Word"));            
        }
    }
}
