namespace api.makebe.agenda.domain.Entidades
{
    public class PortifolioItem
    {
        public int Id { get; set; }
        public int PortifolioId { get; set; }
        public bool Status { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
