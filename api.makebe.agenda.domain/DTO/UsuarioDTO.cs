namespace api.makebe.agenda.domain.DTO
{
    public class UsuarioDTO
    {
        public string? Id { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Instagran { get; set; }
        public string? PermissaoId { get; set; }
        public string? NomeImagem { get; set; }
        public string? UrlImagem { get; set; }
        public bool? Status { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
