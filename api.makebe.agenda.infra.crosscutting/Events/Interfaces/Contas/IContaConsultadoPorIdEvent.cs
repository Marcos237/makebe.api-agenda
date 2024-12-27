using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;

namespace ContasEvent
{
    public interface IContaConsultadoPorIdEvent
    {
        Guid Id { get; set; }
        ContaEvent? ContaEvent { get; set; }
        DateTime DataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
