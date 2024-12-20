using api.makebe.agenda.infra.crosscutting.Notifications;
using api.makebesession.infra.crosscutting.Entidades;
using api.makebesession.infra.crosscutting.Events.Interfaces.Conta;

namespace api.makebesession.infra.crosscutting.Events.Conta
{
    public class UsuarioContaRegistradoEvent : IUsuarioContaRegistradoEvent
    {
        public UsuarioContaEvent? Conta { get; set; }
        public DateTime dataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
