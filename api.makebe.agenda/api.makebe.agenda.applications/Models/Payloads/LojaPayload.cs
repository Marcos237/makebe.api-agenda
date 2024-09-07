using api.makebe.agenda.applications.Models.Responses;

namespace api.makebe.agenda.applications.Models.Payloads
{
    public class LojaPayload
    {
        public int Id { get; set; }
        public string? RazaoSocial { get; set; }
        public string? CNPJ { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public int TipoLojaId { get; set; }

        public IEnumerable<EnderecoRespose>? Enderecos { get; set; }

        public LojaPayload()
        {
            Enderecos = new List<EnderecoRespose>();
        }
    }
}
