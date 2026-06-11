using MassTransit;

namespace EnderecoLojaEvent
{
    [EntityName("endereco-loja-publicados")]
    public class EnderecoLojaPublicadoEvent : IEnderecoLojaPublicadoEvent
    {
        public int Id { get; set; }
        public IEnumerable<EnderecoLojaEvent> Lojas { get; set; } = Enumerable.Empty<EnderecoLojaEvent>();
        public DateTime DataEvento { get; set; } = DateTime.Now;
    }
}
