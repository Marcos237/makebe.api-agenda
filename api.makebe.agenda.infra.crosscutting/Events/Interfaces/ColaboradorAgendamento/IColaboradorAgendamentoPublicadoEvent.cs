namespace ColaboradorAgendamentoEvent
{
    public interface IColaboradorAgendamentoPublicadoEvent
    {
        int ColaboradorId { get; set; }
        IEnumerable<ColaboradorAgendamentoEvent> Agendamentos { get; set; }
        DateTime DataEvento { get; set; }
    }
}
