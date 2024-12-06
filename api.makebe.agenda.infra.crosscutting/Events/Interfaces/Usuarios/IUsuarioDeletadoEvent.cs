using api.makebe.agenda.infra.crosscutting.Notifications;

namespace api.makebesession.infra.crosscutting.Events.Interfaces.Usuarios
{
    public interface IUsuarioDeletadoEvent
    {
        public Guid Id { get; set; }
        DateTime dataEvento { get; set; }
        bool IsDeletado { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
