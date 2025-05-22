namespace api.makebe.agenda.applications.Models.Payloads
{
    public class AgendaPayload
    {
        public int Id { get; set; }
        public bool IsTodoDia { get; set; }
        public int IdAgendaSemanaInicio { get; set; }
        public int IdAgendaSemanaFim { get; set; }
        public string? AgendaAbertaInicio { get; set; }
        public string? AgendaAbertaFim { get; set; }
        public string? AgendaBloqueadaInicio { get; set; }
        public string? AgendaBloqueadaFim { get; set; }
        public bool IsBloqueadoHoje { get; set; }
        public int IdLoja { get; set; }
        public int IdColaborador { get; set; }
        public bool Bloqueado { get; set; }
        public int Tipo { get; set; }
        public string? RazaoSocial { get; set; }
        public string? Nome { get; set; }

    }
}
