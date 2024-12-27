using api.makebe.agenda.infra.crosscutting.Notifications;

namespace ContasEvent
{
    public interface IUsuarioContaDeletadoEvent
    {
        public Guid? Id { get; set; }
        DateTime dataEvento { get; set; }
        bool IsDeletado { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
