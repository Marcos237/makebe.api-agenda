using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;

namespace ContasEvent
{
    public class UsuarioContaRegistradoEvent : IUsuarioContaRegistradoEvent
    {
        public UsuarioContaEvent? Conta { get; set; }
        public DateTime dataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
