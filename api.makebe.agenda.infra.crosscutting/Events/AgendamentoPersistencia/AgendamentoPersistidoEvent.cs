using api.makebe.agenda.infra.crosscutting.Notifications;
using MassTransit;

namespace AgendamentoPersistenciaEvent
{
    [EntityName("agendamento-persistidos")]
    public class AgendamentoPersistidoEvent : IAgendamentoPersistidoEvent
    {
        public AgendamentoPersistenciaEvent? Agendamento { get; set; }
        public string? UsuarioIdEvent { get; set; }
        public IEnumerable<Notification> Notifications { get; set; } = Enumerable.Empty<Notification>();
        public DateTime DataEvento { get; set; } = DateTime.Now;
    }
}
