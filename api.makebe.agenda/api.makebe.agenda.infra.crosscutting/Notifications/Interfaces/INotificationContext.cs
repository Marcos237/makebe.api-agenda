namespace api.makebe.agenda.infra.crosscutting.Notifications.Interfaces
{
    public interface INotificationContext
    {
        IReadOnlyCollection<Notification> Notifications { get; }
        public bool HasNotifications { get; }
        void AddNotification(string key, string message, bool isValidate = false);
        void AddNotification(Notification notification);
        void AddNotifications(IEnumerable<Notification> notifications);
    }
}
