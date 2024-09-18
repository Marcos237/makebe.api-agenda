namespace api.makebe.agenda.domain.Entidades
{
    public class TipoLoja
    {
        public int Id { get; set; }
        public string?  Descricao { get; set; }
        public bool Status { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
