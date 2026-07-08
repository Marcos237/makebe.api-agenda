namespace api.makebe.agenda.domain.Entidades
{
    public class CategoriaItem
    {
        public int Id { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; }

        public bool Status { get; set; }
    }
}
