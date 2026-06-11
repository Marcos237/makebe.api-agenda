using api.makebe.agenda.infra.crosscutting.Notifications;

namespace AgendamentoPersistenciaEvent
{
    public interface IAgendamentoPersistidoEvent
    {
        AgendamentoPersistenciaEvent? Agendamento { get; set; }
        string? UsuarioIdEvent { get; set; }
        IEnumerable<Notification> Notifications { get; set; }
        DateTime DataEvento { get; set; }
    }
}
