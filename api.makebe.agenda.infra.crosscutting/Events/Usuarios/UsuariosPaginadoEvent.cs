using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;
using api.makebesession.infra.crosscutting.Entidades;
using api.makebesession.infra.crosscutting.Events.Interfaces.Usuarios;

namespace api.makebesession.infra.crosscutting.Events.Usuarios
{
    public class UsuariosPaginadoEvent : IUsuarioPaginadoEvent
    {
        public PaginacaoEvent<UsuarioEvent> paginacao { get; set; } = new PaginacaoEvent<UsuarioEvent>();
        public IEnumerable<UsuarioEvent> UsuarioConsultadoRetorno { get; set; } = Enumerable.Empty<UsuarioEvent>();

        public DateTime dataEvento { get; set; } = DateTime.Now;
        public IEnumerable<Notification>? NotificationContext { get; set; } = Enumerable.Empty<Notification>();
    }
}
