using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.DTO
{
    public class AgendaDTO
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }
        public int IdAgendaColaborador { get; set; }
        public string? UsuarioId { get; set; }
        public string? ContaId { get; set; }
        public string? RazaoSocial { get; set; }
        public string? Nome { get; set; }
        public string? AgendaAbertaInicio { get; set; }
        public string? AgendaAbertaFim { get; set; }
        public bool Bloqueado { get; set; }
        public bool IsBloqueadoHoje { get; set; }
        public bool IsTodoDia { get; set; }
        public int IdAgendaSemanaInicio { get; set; }
        public int IdAgendaSemanaFim { get; set; }
        public string? AgendaBloqueadaInicio { get; set; }
        public string? AgendaBloqueadaFim { get; set; }
        public string? DiaInicioSemana { get; set; }
        public string? DiaSemanaFim { get; set; }
        public int IdColaborador { get; set; }
        public string? DataInicioAgendamento { get; set; }
        public string? DataTerminoAgendamento { get; set; }
    }
}
