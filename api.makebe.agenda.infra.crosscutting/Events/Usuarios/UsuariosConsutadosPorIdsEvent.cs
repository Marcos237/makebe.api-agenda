using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;

namespace UsuariosEvent
{
    public class UsuariosConsutadosPorIdsEvent : IUsuariosConsutadosPorIdsEvent
    {
        public IEnumerable<string>? Ids { get; set; }
        public IEnumerable<UsuarioEvent>? UsuariosConsultadosRetorno { get; set; }
        public DateTime dataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
