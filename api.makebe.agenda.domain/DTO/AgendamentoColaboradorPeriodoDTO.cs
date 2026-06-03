namespace api.makebe.agenda.domain.DTO
{
    public class AgendamentoColaboradorPeriodoDTO
    {
        public int IdAgendaColaborador { get; set; }
        public int ColaboradorId { get; set; }
        public DateTime PeriodoInativoInicio { get; set; }
        public DateTime PeriodoInativoFim { get; set; }
        public DateTime AgendaBloqueadaInicio { get; set; }
        public DateTime AgendaBloqueadaFim { get; set; }
        public DateTime? DataInicioAgendamento { get; set; }
        public DateTime? DataTerminoAgendamento { get; set; }
    }
}
