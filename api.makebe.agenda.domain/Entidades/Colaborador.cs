namespace api.makebe.agenda.domain.Entidades
{
    public  class Colaborador
    {
        public int Id { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime Datacadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool Status { get; set; }
        public bool IsGestor { get; set; }
    }
}
