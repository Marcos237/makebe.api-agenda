using api.makebe.agenda.infra.crosscutting.Notifications;
using api.makebesession.infra.crosscutting.Entidades;
using api.makebesession.infra.crosscutting.Events.Interfaces.Usuarios;

namespace api.makebesession.infra.crosscutting.Events.Usuarios
{
    public class UsuarioConsultadoPorIdEvent : IUsuarioConsultadoPorIdEvent
    {
        public Guid Id { get; set; }
        public UsuarioEvent? UsuarioConsultadoRetorno { get; set; }
        public DateTime dataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
