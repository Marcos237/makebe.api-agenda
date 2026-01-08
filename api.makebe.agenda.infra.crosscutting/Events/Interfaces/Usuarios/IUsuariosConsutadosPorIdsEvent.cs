using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;

namespace UsuariosEvent
{
    internal interface IUsuariosConsutadosPorIdsEvent
    {
        IEnumerable<string>? Ids { get; set; }
        IEnumerable<UsuarioEvent> UsuariosConsultadosRetorno { get; set; }
        DateTime dataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
