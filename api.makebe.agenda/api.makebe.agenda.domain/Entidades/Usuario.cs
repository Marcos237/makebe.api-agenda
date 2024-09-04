namespace api.makebe.agenda.domain.Entidades
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Instagran { get; set; }
        public Guid PermissaoId { get; set; }
        public bool Status { get; set; }
        public bool MostrarVitrine { get; set; }
        public string? Senha { get; set; }
        public string? ConfirmaSenha { get; set; }
        public string? NomeImagem { get; set; }
        public string? UrlImagem { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
