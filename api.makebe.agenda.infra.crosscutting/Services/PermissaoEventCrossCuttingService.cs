using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using PermissoesEvent;

namespace api.makebe.agenda.infra.crosscutting.Services
{
    public class PermissaoEventCrossCuttingService : IPermissaoEventCrossCuttingService
    {
        private readonly IBusEvent _busEvent;
        public PermissaoEventCrossCuttingService(IBusEvent busEvent)
        {
            _busEvent = busEvent;
        }
        public async Task<PermissoesConsultadasEvent> BuscarPermissoes(PermissoesConsultadasEvent permissoesConsultadasEvent)
        {
            var permissoes = await _busEvent.RequestAsync<PermissoesConsultadasEvent, PermissoesConsultadasEvent>(permissoesConsultadasEvent, TimeSpan.FromSeconds(15));
            return permissoes;
        }
    }
}
