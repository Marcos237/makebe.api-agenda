namespace api.makebe.agenda.domain.Entidades
{
    public class Agendamento
    {
        public int Id { get; set; }
        public int IdAgendaColaborado { get; set; }
        public int IdServico { get; set; }
        public string? IdUsuario { get; set; }
        public DateTime DataInicioAgendamento { get; set; }
        public DateTime DataTerminoAgendamento { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
