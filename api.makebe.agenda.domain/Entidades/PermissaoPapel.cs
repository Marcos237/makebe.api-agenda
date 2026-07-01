namespace api.makebe.agenda.domain.Entidades
{
    public class PermissaoPapel
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; }
        public Guid PapeisId { get; set; }
        public string? Papeis { get; set; }
    }
}
