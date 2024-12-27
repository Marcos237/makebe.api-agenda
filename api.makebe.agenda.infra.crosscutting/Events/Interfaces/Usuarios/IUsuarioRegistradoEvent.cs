using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;

namespace UsuariosEvent
{
    internal interface IUsuarioRegistradoEvent
    {
        UsuarioEvent UsuarioConsultado { get; set; }
        DateTime dataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
