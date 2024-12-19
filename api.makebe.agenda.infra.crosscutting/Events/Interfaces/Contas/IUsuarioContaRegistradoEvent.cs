using api.makebe.agenda.infra.crosscutting.Notifications;
using api.makebesession.infra.crosscutting.Entidades;

namespace api.makebesession.infra.crosscutting.Events.Interfaces.Conta
{
    public interface IUsuarioContaRegistradoEvent
    {
        UsuarioContaEvent Conta { get; set; }
        DateTime dataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
