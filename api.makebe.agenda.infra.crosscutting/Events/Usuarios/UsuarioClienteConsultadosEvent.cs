using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;

namespace UsuariosEvent
{
    public class UsuarioClienteConsultadorEvent : IUsuarioClienteConsultadosEvent
    {
        public IEnumerable<UsuarioEvent>? UsuariosEvents { get; set; }
        public DateTime DataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
