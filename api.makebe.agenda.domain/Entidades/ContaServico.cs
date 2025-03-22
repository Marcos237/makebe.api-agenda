namespace api.makebe.agenda.domain.Entidades
{
    public class ContaServico
    {
        public int Codigo { get; set; }
        public string? ContaId { get; set; }
        public int ServicoId { get; set; }
        public bool Status { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
