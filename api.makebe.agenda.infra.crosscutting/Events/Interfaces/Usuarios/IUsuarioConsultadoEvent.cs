using api.makebe.agenda.infra.crosscutting.Notifications;
using api.makebesession.infra.crosscutting.Entidades;

namespace api.makebesession.infra.crosscutting.Events.Interfaces
{
    public interface IUsuarioConsultadoEvent
    {
        UsuarioEvent UsuarioConsultado { get; set; }
        IEnumerable<UsuarioEvent> UsuarioConsultadoRetorno { get; set; }
        DateTime dataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
