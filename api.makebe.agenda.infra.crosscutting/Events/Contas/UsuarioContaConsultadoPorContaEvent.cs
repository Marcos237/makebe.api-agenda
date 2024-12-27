using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;

namespace ContasEvent
{
    public class UsuarioContaConsultadoPorContaEvent : IUsuarioContaConsultadoPorContaEvent
    {
        public Guid IdConta { get; set; }
        public IEnumerable<UsuarioEvent>? UsuariosEvents { get; set; }
        public DateTime DataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
