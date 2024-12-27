using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;

namespace UsuariosEvent
{
    public class UsuarioConsultadoPorIdEvent : IUsuarioConsultadoPorIdEvent
    {
        public Guid Id { get; set; }
        public UsuarioEvent? UsuarioConsultadoRetorno { get; set; }
        public DateTime dataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
