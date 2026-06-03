using MassTransit;

namespace PeriodoDisponivelAgendamentoEvent
{
    [EntityName("periodo-disponivel-agendamento-publicados")]
    public class PeriodoDisponivelAgendamentoPublicadoEvent : IPeriodoDisponivelAgendamentoPublicadoEvent
    {
        public int IdServico { get; set; }
        public int IdColaborador { get; set; }
        public DateTime Data { get; set; }
        public IEnumerable<PeriodoEvent> Periodos { get; set; } = Enumerable.Empty<PeriodoEvent>();
        public DateTime DataEvento { get; set; } = DateTime.Now;
    }
}
