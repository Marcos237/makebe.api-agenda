namespace api.makebe.agenda.domain.Entidades
{
    public  class EnderecoItem
    {
        public int Id { get; set; }
        public int EnderecoId { get; set; }
        public bool Status { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
