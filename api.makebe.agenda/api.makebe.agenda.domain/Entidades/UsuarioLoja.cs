namespace api.makebe.agenda.domain.Entidades
{
    public class UsuarioLoja
    {
        public int Id { get; set; }
        public string? UsuarioId { get; set; }
        public int LojaId { get; set; }
        public bool Status { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
