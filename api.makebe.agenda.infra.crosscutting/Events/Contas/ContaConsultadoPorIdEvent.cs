using api.makebe.agenda.infra.crosscutting.Notifications;
using api.makebesession.infra.crosscutting.Entidades;
using api.makebesession.infra.crosscutting.Events.Interfaces.Conta;

namespace api.makebesession.infra.crosscutting.Events.Contas
{
    public class ContaConsultadoPorIdEvent : IContaConsultadoPorIdEvent
    {
        public Guid Id { get; set; }
        public ContaEvent? ContaEvent { get; set; }
        public DateTime DataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
