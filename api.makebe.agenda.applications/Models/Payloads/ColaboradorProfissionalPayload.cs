namespace api.makebe.agenda.applications.Models.Payloads
{
    public class ColaboradorProfissionalPayload
    {
        public int Id { get; set; }
        public int ColaboradorId { get; set; }
        public int LojaId { get; set; }
        public int ServicoId { get; set; }
        public IEnumerable<api.makebe.agenda.domain.Entidades.ColaboradorServicos>? Servicos { get; set; }
        public string? Descricao { get; set; }
        public DateTime PeriodoInativoInicio { get; set; }
        public DateTime PeriodoInativoFim { get; set; }
        public string? PeriodoInativoInicioExtenso { get; set; }
        public string? PeriodoInativoFimExtenso { get; set; }
    }
}
