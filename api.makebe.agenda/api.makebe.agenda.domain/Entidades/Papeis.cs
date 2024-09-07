namespace api.makebe.agenda.domain.Entidades
{
    public  class Papeis
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Status { get; set; }
    }
}
