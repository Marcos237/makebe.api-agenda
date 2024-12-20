namespace api.makebe.agenda.domain.DTO
{
    public class LojaDTO
    {
        public int Id { get; set; }
        public string? RazaoSocial { get; set; }
        public string? CNPJ { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public bool Status { get; set; }
        public int TipoLojaId { get; set; }
        public string? TipoLojaDescricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
