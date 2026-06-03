namespace ColaboradorAgendamentoEvent
{
    public class ColaboradorAgendamentoEvent
    {
        public int Id { get; set; }
        public int IdAgendaColaborador { get; set; }
        public bool IsBloqueadoHoje { get; set; }
        public string? AgendaAbertaInicio { get; set; }
        public string? AgendaAbertaFim { get; set; }
        public string? AgendaBloqueadaInicio { get; set; }
        public string? AgendaBloqueadaFim { get; set; }
        public string? UsuarioId { get; set; }
        public string? DataInicioAgendamento { get; set; }
        public string? DataTerminoAgendamento { get; set; }
        public IEnumerable<ServicoEvent> Servicos { get; set; } = Enumerable.Empty<ServicoEvent>();
        public IEnumerable<AgendamentoEvent> Agendamentos { get; set; } = Enumerable.Empty<AgendamentoEvent>(); 
    }
}
