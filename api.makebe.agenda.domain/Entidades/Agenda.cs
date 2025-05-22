namespace api.makebe.agenda.domain.Entidades
{
    public class Agenda
    {
        public int Id { get; set; }
        public bool IsTodoDia { get; set; }
        public int IdAgendaSemanaInicio { get; set; }
        public int IdAgendaSemanaFim { get; set; }
        public DateTime? AgendaAbertaInicio { get; set; }
        public DateTime? AgendaAbertaFim { get; set; }
        public bool IsBloqueadoHoje { get; set; }
        public DateTime? AgendaBloqueadaInicio { get; set; }
        public DateTime? AgendaBloqueadaFim { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool Status { get; set; }

        public IEnumerable<AgendaLoja> AgendaLojas { get; set; }

        public Agenda()
        {
            AgendaLojas = new List<AgendaLoja>();
        }
    }

}
