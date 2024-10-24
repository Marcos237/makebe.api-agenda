namespace api.makebe.agenda.domain.DTO
{
    public class EnderecoDTO
    {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public string? RazaoSocial { get; set; }
        public string? Logradouro { get; set; }
        public int Numero { get; set; }
        public string? Complemento { get; set; }
        public string? CEP { get; set; }
        public string? Estado { get; set; }
        public string? Cidade { get; set; }

    }
}
