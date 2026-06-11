using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using LojasEvent;

namespace api.makebe.agenda.infra.crosscutting.Services
{
    public class LojaEventCrossCuttingService : ILojaEventCrossCuttingService
    {
        private readonly IBusEvent _busEvent;

        public LojaEventCrossCuttingService(IBusEvent busEvent)
        {
            _busEvent = busEvent;
        }

        public async Task PublicarLojasVitrine(LojasVitrinePublicadasEvent lojasVitrinePublicadasEvent)
        {
            await _busEvent.PublishAsync(lojasVitrinePublicadasEvent);
        }
    }
}
