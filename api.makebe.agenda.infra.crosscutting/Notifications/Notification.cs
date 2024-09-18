namespace api.makebe.agenda.infra.crosscutting.Notifications
{
    public class Notification
    {
        public string Key { get; }
        public string Message { get; }
        public bool IsValidate { get; }

        public Notification(string key, string message, bool isValidate)
        {
            Key = key;
            Message = message;
            IsValidate = isValidate;
        }
    }
}
