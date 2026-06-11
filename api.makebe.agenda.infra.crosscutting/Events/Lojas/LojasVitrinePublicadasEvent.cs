using MassTransit;

namespace LojasEvent
{
    [EntityName("lojas-vitrine-publicadas")]
    public class LojasVitrinePublicadasEvent : ILojasVitrinePublicadasEvent
    {
        public string Tipo { get; set; } = string.Empty;
        public IEnumerable<LojaVitrineEvent> Lojas { get; set; } = Enumerable.Empty<LojaVitrineEvent>();
        public DateTime DataEvento { get; set; } = DateTime.Now;
    }
}
