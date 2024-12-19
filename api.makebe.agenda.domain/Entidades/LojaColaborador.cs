namespace api.makebe.agenda.domain.Entidades
{
    public class LojaColaborador
    {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public int ColaboradorId { get; set; }
        public bool Status { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
