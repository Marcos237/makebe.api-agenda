namespace api.makebe.agenda.domain.DTO
{
    public class AgendamentoDTO
    {
        public int Id { get; set; }
        public int IdLoja { get; set; }
        public string? RazaoSocial { get; set; }
        public int IdAgendaColaborador { get; set; }
        public string? IdColaborador { get; set; }
        public string? NomeColaborador { get; set; }
        public int IdServico { get; set; }
        public string? DescricaoServico { get; set; }
        public decimal Valor { get; set; }
        public string? IdUsuario { get; set; }
        public string? NomeUsuario { get; set; }
        public string? Data { get; set; }
        public DateTime DataInicioAgendamento { get; set; }
        public DateTime DataTerminoAgendamento { get; set; }
        public string? DataInicioAgendamentoExtenso { get; set; }
        public DateTime PeriodoInativoInicio { get; set; }
        public DateTime PeriodoInativoFim { get; set; }
        public bool Ativo { get; set; }
        public bool IsBloqueadoHoje { get; set; }
        public Decimal Periodo { get; set; }
        public string? NomeCliente { get; set; }
        public string? TelefoneCliente { get; set; }
    }
}
