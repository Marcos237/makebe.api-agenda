using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using UsuariosEvent;

namespace api.makebe.agenda.infra.crosscutting.Services
{
    public class UsuarioClienteConsultadosCrosCuttingService : IUsuarioClienteConsultadosCrosCuttingService
    {
        private readonly IBusEvent _busEvent;
        public UsuarioClienteConsultadosCrosCuttingService(IBusEvent busEvent)
        {
            _busEvent = busEvent;
        }
        public async  Task<IEnumerable<UsuarioEvent>> BuscarUsuarioClientes()
        {
            var usuarioClienteEvent = new  UsuarioClienteConsultadorEvent();
            var usuarios = await _busEvent.RequestAsync<UsuarioClienteConsultadorEvent, UsuarioClienteConsultadorEvent>(usuarioClienteEvent, TimeSpan.FromSeconds(15));
            return usuarios?.UsuariosEvents ?? Enumerable.Empty<UsuarioEvent>();
        }
    }
}
