namespace api.makebe.agenda.applications.Models.Payloads
{
    public  class ColaboradorPayload : BasePayload
    {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public Guid UsuarioId { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Instagran { get; set; }
        public Guid PermissaoId { get; set; }
        public string? NomeImagem { get; set; }
        public string? UrlImagem { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
