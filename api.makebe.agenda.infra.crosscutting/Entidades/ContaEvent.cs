namespace api.makebesession.infra.crosscutting.Entidades
{
    public class ContaEvent
    {
        public Guid? Id { get; set; }
        public Guid? UsuarioId { get; set; }
        public string? UrlInicial { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Status { get; set; }
    }
}
