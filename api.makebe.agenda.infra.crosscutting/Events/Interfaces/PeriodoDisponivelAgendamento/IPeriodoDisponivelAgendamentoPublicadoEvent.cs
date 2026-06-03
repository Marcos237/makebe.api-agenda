namespace PeriodoDisponivelAgendamentoEvent
{
    public interface IPeriodoDisponivelAgendamentoPublicadoEvent
    {
        int IdServico { get; set; }
        int IdColaborador { get; set; }
        DateTime Data { get; set; }
        IEnumerable<PeriodoEvent> Periodos { get; set; }
        DateTime DataEvento { get; set; }
    }
}
