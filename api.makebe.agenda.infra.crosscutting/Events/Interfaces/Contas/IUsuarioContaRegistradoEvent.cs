using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;

namespace ContasEvent
{
    public interface IUsuarioContaRegistradoEvent
    {
        UsuarioContaEvent Conta { get; set; }
        DateTime dataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
