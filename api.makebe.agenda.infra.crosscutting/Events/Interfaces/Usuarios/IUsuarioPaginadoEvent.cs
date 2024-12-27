using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;

namespace UsuariosEvent
{
    public interface IUsuarioPaginadoEvent
    {
        PaginacaoEvent<UsuarioEvent> paginacao { get; set; }
        IEnumerable<UsuarioEvent> UsuarioConsultadoRetorno { get; set; }
        DateTime dataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
