using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;

namespace ContasEvent
{
    public interface IUsuarioContaConsultadoPorContaEvent
    {
        Guid IdConta { get; set; }
        IEnumerable<UsuarioEvent>? UsuariosEvents { get; set; }
        DateTime DataEvento { get; set; }
        public IEnumerable<Notification>? NotificationContext { get; set; }
    }
}
