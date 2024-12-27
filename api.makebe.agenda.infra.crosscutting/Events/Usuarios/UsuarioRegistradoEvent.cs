using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;

namespace UsuariosEvent
{
    public class UsuarioRegistradoEvent : IUsuarioRegistradoEvent
    {
        public UsuarioEvent UsuarioConsultado { get; set; } = new UsuarioEvent();
        public DateTime dataEvento { get; set; } = DateTime.Now;
        public IEnumerable<Notification>? NotificationContext { get; set; } = Enumerable.Empty<Notification>();
    }
}
