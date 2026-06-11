using MassTransit;

namespace ColaboradorAgendamentoEvent
{
    [EntityName("colaborador-agendamento-publicados")]
    public class ColaboradorAgendamentoPublicadoEvent : IColaboradorAgendamentoPublicadoEvent
    {
        public int ColaboradorId { get; set; }
        public IEnumerable<ColaboradorAgendamentoEvent> Agendamentos { get; set; } = Enumerable.Empty<ColaboradorAgendamentoEvent>();
        public DateTime DataEvento { get; set; } = DateTime.Now;
    }
}
