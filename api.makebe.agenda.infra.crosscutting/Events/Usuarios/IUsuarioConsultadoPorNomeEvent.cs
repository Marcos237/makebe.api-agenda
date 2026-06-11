using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;

namespace UsuariosEvent
{
    public interface IUsuarioConsultadoPorNomeEvent
    {
        string? Nome { get; set; }
        UsuarioEvent? UsuarioConsultadoRetorno { get; set; }
        DateTime dataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
