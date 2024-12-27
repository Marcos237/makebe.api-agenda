using api.makebe.agenda.infra.crosscutting.Notifications;

namespace ContasEvent
{
    public class UsuarioContaDeletadoEvent : IUsuarioContaDeletadoEvent
    {
        public Guid? Id { get; set; }
        public DateTime dataEvento { get; set; }
        public bool IsDeletado { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
