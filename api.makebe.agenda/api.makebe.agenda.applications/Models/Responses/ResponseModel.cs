using api.makebe.agenda.infra.crosscutting.Notifications;

namespace api.makebe.agenda.applications.Models.Responses
{
    public class ResponseModel<T> where T : class
    {
        public string? Message { get; set; }
        public IEnumerable<Notification>? notifications { get; set; }
        public T? data { get; set; }
        public IEnumerable<T>? datas { get; set; }
    }
}
