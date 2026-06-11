using api.makebe.agenda.infra.crosscutting.Entidades;
using MassTransit;

namespace MeusAgendamentosEvent
{
    [EntityName("meus-agendamentos-publicados")]
    public class MeusAgendamentosPublicadoEvent : IMeusAgendamentosPublicadoEvent
    {
        public string? UsuarioIdEvent { get; set; }
        public PaginacaoEvent<MeuAgendamentoEvent> Paginacao { get; set; } = new();
        public DateTime DataEvento { get; set; } = DateTime.Now;
    }
}
