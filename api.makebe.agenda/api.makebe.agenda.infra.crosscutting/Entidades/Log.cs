using Newtonsoft.Json;

namespace api.makebe.agenda.infra.crosscutting.Entidades
{
    public class Log
    {
        public int Id { get; set; }
        public string? Metodo { get; set; }
        public string? Mensagem { get; set; }
        public string? Objeto { get; set; }
        public DateTime DataCadastro { get; set; }
        public string? Usuario { get; set; }
        public string? Request { get; set; }
        public string? CamposValidados { get; set; }
        public string? Tipo { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(Mensagem);
        }
    }
}
