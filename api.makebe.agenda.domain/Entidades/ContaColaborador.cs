namespace api.makebe.agenda.domain.Entidades
{
    public class ContaColaborador
    {
        public int Id { get; set; }
        public int ColaboradorId { get; set; }
        public Guid? ContaId { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
