namespace api.makebe.agenda.domain.DTO
{
    public class PermissaoMenuDTO
    {
        public Guid? Id { get; set; }
        public Guid PermissaoId { get; set; }
        public string? MenuDescricao { get; set; }
        public string? MenuUrl { get; set; }
    }
}
