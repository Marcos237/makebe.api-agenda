using api.makebe.agenda.domain.ValueObjects;

namespace api.makebe.agenda.domain.Entidades
{
    public class Loja
    {
        public int Id { get; set; }
        public string? RazaoSocial { get; set; }
        public CNPJ? CNPJ { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public bool Status { get; set; }
        public  DateTime DataCadastro { get; set; }
        public  DateTime DataAtualizacao { get; set; }

    }
}
