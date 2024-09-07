namespace api.makebe.agenda.domain.Entidades
{
    internal class LojaEndereco
    {
        public int Id { get; set; } 
        public int LojaId { get; set; }          
        public int EnderecoId { get; set; }     
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
