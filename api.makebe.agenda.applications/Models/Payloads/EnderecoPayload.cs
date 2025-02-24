namespace api.makebe.agenda.applications.Models.Payloads
{
    public class EnderecoPayload
    {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public string? UsuarioId { get; set; }
        public int ColaboradorId { get; set; }
        public string? Logradouro { get; set; }
        public int Numero { get; set; }
        public string? Complemento { get; set; }
        public string? CEP { get; set; }
        public string? Estado { get; set; }
        public string? Cidade { get; set; }
        public int LojaEnderecoId { get; set; }
        public int ColaboradorEnderecoId { get; set; }
        public int TipoUsuarioId { get; set; }
    }
}
