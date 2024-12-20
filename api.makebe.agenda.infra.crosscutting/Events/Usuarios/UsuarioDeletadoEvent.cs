using api.makebe.agenda.infra.crosscutting.Notifications;
using api.makebesession.infra.crosscutting.Events.Interfaces.Usuarios;

namespace api.makebesession.infra.crosscutting.Events.Usuarios
{
    public class UsuarioDeletadoEvent : IUsuarioDeletadoEvent
    {
        public Guid Id { get; set; }
        public DateTime dataEvento { get; set; }
        public bool IsDeletado { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}