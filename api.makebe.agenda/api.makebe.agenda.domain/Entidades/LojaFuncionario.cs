namespace api.makebe.agenda.domain.Entidades
{
    public class LojaFuncionario
    {
        public int Id { get; set; }
        public string? UsuarioLojaId { get; set; }
        public bool Status { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
