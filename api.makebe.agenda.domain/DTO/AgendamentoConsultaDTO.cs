namespace api.makebe.agenda.domain.DTO
{
    public class AgendamentoConsultaDTO
    {
        public int Id { get; set; }
        public string? IdUsuario { get; set; }
        public DateTime DataInicioAgendamento { get; set; }
        public DateTime DataTerminoAgendamento { get; set; }
        public string? DescricaoServico { get; set; }
        public int IdColaborador { get; set; }
        public string? IdColaboradorUsuario { get; set; }
        public string? NomeColaborador { get; set; }
        public bool EhDesativado { get; set; }
    }
}
