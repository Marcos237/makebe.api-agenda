using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using ContasEvent;

namespace api.makebe.agenda.infra.crosscutting.Services
{
    public class ContaEventCrossCuttingService : IContaEventCrossCuttingService
    {
        private readonly IBusEvent _busEvent;
        public ContaEventCrossCuttingService(IBusEvent busEvent)
        {
            _busEvent = busEvent;
        }
        public async Task<ContaEvent> BuscarContaPorId(Guid usuarioId)
        {
            var ContaConsultadoPorIdEvent = new ContaConsultadoPorIdEvent() { Id = usuarioId };
            var conta = await _busEvent.RequestAsync<ContaConsultadoPorIdEvent, ContaConsultadoPorIdEvent>(ContaConsultadoPorIdEvent, TimeSpan.FromSeconds(15));
            return conta?.ContaEvent;
        }

        public async Task<UsuarioContaConsultadoPorContaEvent> BuscarUsuarioContaPorIdConta(UsuarioContaConsultadoPorContaEvent usuarioContaConsultadoPorContaEvent)
        {
            var usuarioConta = await _busEvent.RequestAsync<UsuarioContaConsultadoPorContaEvent, UsuarioContaConsultadoPorContaEvent>(usuarioContaConsultadoPorContaEvent, TimeSpan.FromSeconds(15));
            return usuarioConta;
        }

        public async Task DeletarContaUsuario(UsuarioContaDeletadoEvent usuarioContaDeletadoEvent)
        {
            await _busEvent.PublishAsync(usuarioContaDeletadoEvent);
        }

        public async Task<UsuarioContaRegistradoEvent> SalvarUsuarioConta(UsuarioContaRegistradoEvent usuarioContaRegistradoEvent)
        {
            var usuarioConta = await _busEvent.RequestAsync<UsuarioContaRegistradoEvent, UsuarioContaRegistradoEvent>(usuarioContaRegistradoEvent, TimeSpan.FromSeconds(15));
            return usuarioConta;
        }
    }
}
