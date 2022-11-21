using Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabbitMq.Producao
{
    public interface IProdutorFila
    {
        Task ExecutarAsync(MensagemModel mensagem);
    }
}
