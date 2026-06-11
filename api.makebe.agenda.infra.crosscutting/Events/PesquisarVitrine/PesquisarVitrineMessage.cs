using MassTransit;

namespace PesquisarVitrineEvent
{
    [EntityName("pesquisar-vitrine")]
    public class PesquisarVitrineMessage : IPesquisarVitrineMessage
    {
        public string Termo { get; set; } = string.Empty;
    }
}
