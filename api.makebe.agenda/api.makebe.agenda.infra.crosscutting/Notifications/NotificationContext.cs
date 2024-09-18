using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;

namespace api.makebe.agenda.infra.crosscutting.Notifications
{
    public class NotificationContext : INotificationContext
    {
        public readonly List<Notification> _notifications;
        public IReadOnlyCollection<Notification> Notifications => _notifications;
        public bool HasNotifications => _notifications.Any();
        public NotificationContext()
        {
            _notifications = new List<Notification>();
        }
        public void AddNotification(string key, string message, bool isValidate)
        {
            _notifications.Add(new Notification(key, message, isValidate));
        }
        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
        }
        public void AddNotifications(IEnumerable<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }
    }
}
