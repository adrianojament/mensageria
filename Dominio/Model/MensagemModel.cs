namespace Dominio.Model
{
    public class MensagemModel
    {
        public MensagemModel(string origem, string conteudo)
        {
            Origem = origem;
            Conteudo = conteudo;
        }

        public Guid Id { get; private init; } = Guid.NewGuid();
        public string Origem { get; private init; }
        public string Conteudo { get; private init; }
        public DateTime CriadoEm { get; private init; } = DateTime.Now;
    }
}