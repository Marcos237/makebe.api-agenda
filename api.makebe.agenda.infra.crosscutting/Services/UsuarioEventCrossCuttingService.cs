using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using UsuariosEvent;

namespace api.makebe.agenda.infra.crosscutting.Services
{
    public class UsuarioEventCrossCuttingService : IUsuarioEventCrossCuttingService
    {
        private readonly IBusEvent _busEvent;
        public UsuarioEventCrossCuttingService(IBusEvent busEvent)
        {
            _busEvent = busEvent;
        }
        public async Task<UsuariosPaginadoEvent> BuscarPaginado(UsuariosPaginadoEvent usuariosPaginadoEvent)
        {
            var usuario = await _busEvent.RequestAsync<UsuariosPaginadoEvent, UsuariosPaginadoEvent>(usuariosPaginadoEvent, TimeSpan.FromSeconds(15));
            return usuario;
        }

        public async Task<UsuarioConsultadoPorIdEvent> BuscarUsuarioPorId(UsuarioConsultadoPorIdEvent usuarioConsultadoPorIdEvent)
        {
            var usuario = await _busEvent.RequestAsync<UsuarioConsultadoPorIdEvent, UsuarioConsultadoPorIdEvent>(usuarioConsultadoPorIdEvent, TimeSpan.FromSeconds(15));
            return usuario;
        }

        public async Task DeletarUsuario(UsuarioDeletadoEvent usuarioDeletadoEvent)
        {
            await _busEvent.PublishAsync(usuarioDeletadoEvent);
        }

        public async Task<UsuarioRegistradoEvent> SalvarUsuario(UsuarioRegistradoEvent usuarioRegistradoEvent)
        {
            var usuario = await _busEvent.RequestAsync<UsuarioRegistradoEvent, UsuarioRegistradoEvent>(usuarioRegistradoEvent, TimeSpan.FromSeconds(15));
            return usuario;
        }
    }
}
