namespace api.makebe.agenda.infra.crosscutting.Entidades
{
    public class UsuarioContaEvent
    {
        public Guid? Id { get; set; }
        public Guid? UsuarioId { get; set; }
        public Guid? ContaId { get; set; }
        public Guid? TipoId { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Status { get; set; }
    }
}
