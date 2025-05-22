namespace api.makebe.agenda.domain.Entidades
{
    public class AgendaLoja
    {
        public int Id { get; set; }
        public int IdAgenda { get; set; }
        public int IdLoja { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool Status { get; set; }

        public IEnumerable<AgendaColaborador> AgendaColaboradores { get; set; }

        public AgendaLoja()
        {
            AgendaColaboradores = new List<AgendaColaborador>();
        }
    }
}
