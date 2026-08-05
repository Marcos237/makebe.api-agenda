namespace api.makebe.agenda.domain.Entidades
{
    public class ColaboradorProfissional
    {
        public int Id { get; set; }
        public int ColaboradorId { get; set; }
        public int LojaId { get; set; }
        public int ServicoId { get; set; }
        public IEnumerable<ColaboradorServicos>? Servicos { get; set; }
        public string? Descricao { get; set; }
        public bool Status { get; set; }
        public DateTime DataCadastro  { get; set; }
        public DateTime DataAtualizacao  { get; set; }
        public DateTime? PeriodoInativoInicio { get; set; }
        public DateTime? PeriodoInativoFim { get; set; } 
    }
}
