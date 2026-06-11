namespace ColaboradorAgendamentoEvent
{
    public class AgendamentoEvent
    {
        public int Id { get; set; }
        public int IdAgendaColaborador { get; set; }
        public int IdServico { get; set; }
        public string? IdUsuario { get; set; }
        public int IdColaborador { get; set; }
        public string? DescricaoServico { get; set; }
        public decimal Valor { get; set; }
        public decimal Periodo { get; set; }
        public DateTime DataInicioAgendamento { get; set; }
        public DateTime DataTerminoAgendamento { get; set; }
    }
}
