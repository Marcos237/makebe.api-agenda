using api.makebe.agenda.infra.crosscutting.Notifications;

namespace UsuariosEvent
{
    public class UsuarioDeletadoEvent : IUsuarioDeletadoEvent
    {
        public Guid Id { get; set; }
        public DateTime dataEvento { get; set; }
        public bool IsDeletado { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}