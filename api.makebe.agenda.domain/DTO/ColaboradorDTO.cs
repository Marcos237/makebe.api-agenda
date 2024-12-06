using lib.makebe.domain.Entidades;

namespace api.makebe.agenda.domain.DTO
{
    public class ColaboradorDTO
    {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public Guid UsuarioId { get; set; }
        public bool Status { get; set; }
        public UsuarioDTO? Usuario { get; set; }
    }
}
