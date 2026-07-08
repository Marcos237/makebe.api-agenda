namespace api.makebe.agenda.domain.Entidades
{
    public class Categoria
    {
        public int Id { get; set; }
        public int ServicoId { get; set; }
        public int CategoriaItemId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
    }
}
