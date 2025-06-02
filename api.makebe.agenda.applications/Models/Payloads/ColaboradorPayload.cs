namespace api.makebe.agenda.applications.Models.Payloads
{
    public  class ColaboradorPayload 
    {
        public int Id { get; set; }
        public string? UsuarioId { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? PermissaoId { get; set; }
        public string? NomeImagem { get; set; }
        public string? UrlImagem { get; set; }
        public bool Status { get; set; }
        public string? Instagram { get; set; }
        public int Tipo { get; set; }
    }
}
