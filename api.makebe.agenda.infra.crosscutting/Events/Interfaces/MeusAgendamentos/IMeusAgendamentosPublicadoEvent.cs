namespace MeusAgendamentosEvent
{
    public interface IMeusAgendamentosPublicadoEvent
    {
        string? UsuarioIdEvent { get; set; }
        api.makebe.agenda.infra.crosscutting.Entidades.PaginacaoEvent<MeuAgendamentoEvent> Paginacao { get; set; }
        DateTime DataEvento { get; set; }
    }
}
