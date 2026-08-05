namespace api.makebe.agenda.domain.Entidades
{
    public class ColaboradorServicos
    {
        public int Id { get; set; }
        public int IdColaborador { get; set; }
        public int IdServico { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool ativo { get; set; }
    }
}
