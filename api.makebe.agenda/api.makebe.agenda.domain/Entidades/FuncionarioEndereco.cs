namespace api.makebe.agenda.domain.Entidades
{
    public class FuncionarioEndereco
    {
        public int Id { get; set; }
        public int LojaFuncionarioId { get; set; }
        public int EnderecoId { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
