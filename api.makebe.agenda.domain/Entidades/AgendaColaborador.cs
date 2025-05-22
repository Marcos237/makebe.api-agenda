namespace api.makebe.agenda.domain.Entidades
{
    public class AgendaColaborador
    {
        public int Id { get; set; }
        public int IdAgenda { get; set; }
        public int IdColaborador { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool Status { get; set; }
    }
}
