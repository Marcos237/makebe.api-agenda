namespace api.makebe.agenda.domain.Entidades
{
    public class ColaboradorEndereco
    {
        public int Id { get; set; }
        public int LojaColaboradorId { get; set; }
        public int EnderecoId { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool Status { get; set; }
    }
}
