namespace api.makebe.agenda.domain.DTO
{
    public class ServicoDTO
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public string? DataCadastro { get; set; }
        public decimal Periodo { get; set; }
        public decimal Valor { get; set; }
        public string? PeriodoExtenso { get; set; }
        public string? ValorExtenso { get; set; }
    }
}
