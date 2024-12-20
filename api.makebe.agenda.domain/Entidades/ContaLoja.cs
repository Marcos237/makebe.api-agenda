namespace api.makebe.agenda.domain.Entidades
{
    public class ContaLoja
    {
        public int Id { get; set; }
        public Guid? ContaId { get; set; }
        public string? Cnpj { get; set; }
        public int LojaId { get; set; }
        public bool Status { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
