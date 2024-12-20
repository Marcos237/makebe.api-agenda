using api.makebe.agenda.infra.crosscutting.Notifications;
using api.makebesession.infra.crosscutting.Entidades;
using api.makebesession.infra.crosscutting.Events.Interfaces.Usuarios;

namespace api.makebesession.infra.crosscutting.Events.Usuarios
{
    public class UsuarioRegistradoEvent : IUsuarioRegistradoEvent
    {
        public UsuarioEvent UsuarioConsultado { get; set; } = new UsuarioEvent();
        public DateTime dataEvento { get; set; } = DateTime.Now;
        public IEnumerable<Notification>? NotificationContext { get; set; } = Enumerable.Empty<Notification>();
    }
}
