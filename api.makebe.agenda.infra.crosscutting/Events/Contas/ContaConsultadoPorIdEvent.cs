using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;

namespace ContasEvent
{
    public class ContaConsultadoPorIdEvent : IContaConsultadoPorIdEvent
    {
        public Guid Id { get; set; }
        public ContaEvent? ContaEvent { get; set; }
        public DateTime DataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }

}
