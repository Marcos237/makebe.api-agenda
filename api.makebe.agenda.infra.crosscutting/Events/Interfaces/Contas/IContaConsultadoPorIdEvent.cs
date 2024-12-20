using api.makebe.agenda.infra.crosscutting.Notifications;
using api.makebesession.infra.crosscutting.Entidades;

namespace api.makebesession.infra.crosscutting.Events.Interfaces.Conta
{
    public interface IContaConsultadoPorIdEvent
    {
        Guid Id { get; set; }
        ContaEvent? ContaEvent { get; set; }
        DateTime DataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
