namespace api.makebe.agenda.applications.Models.Responses
{
    public  class LojaResponse
    {
        public int Id { get; set; }
        public string? RazaoSocial { get; set; }
        public string? CNPJ { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public int TipoLojaId { get; set; }
        public DateTime DataCadastro { get; set; }
        public IEnumerable<EnderecoRespose>? Enderecos { get; set; }

        public LojaResponse()
        {
            Enderecos = new List<EnderecoRespose>();
        }
    }
}
