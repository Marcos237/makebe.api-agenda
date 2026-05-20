namespace api.makebe.agenda.domain.DTO
{
    public class EnderecoLojaDTO
    {
        public int Id { get; set; }
        public string? RazaoSocial { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? Texto { get; set; }
        public string? CEP { get; set; }
        public string? Cidade { get; set; }
        public string? Logradouro { get; set; }
        public int Numero { get; set; }
        public string? Estado { get; set; }
        public string? Complemento { get; set; }
        public bool Status { get; set; }
    }
}
