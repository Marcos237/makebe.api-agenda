namespace api.makebe.agenda.domain.DTO
{
    public class ColaboradorDTO
    {
        public int Id { get; set; }
        public Guid UsuarioId { get; set; }
        public bool Status { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public string? Instagram { get; set; }
        public string? Telefone { get; set; }
        public string? PermissaoId { get; set; }
        public string? DescricaoPermissao { get; set; }
        public string? NomeImagem { get; set; }
        public string? UrlImagem { get; set; }
        public string? DescricaoStatus { get; set; }
    }
}
