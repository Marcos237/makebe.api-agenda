using LojasEvent;

namespace api.makebe.agenda.infra.crosscutting.Services.Interfaces
{
    public interface ILojaEventCrossCuttingService
    {
        Task PublicarLojasVitrine(LojasVitrinePublicadasEvent lojasVitrinePublicadasEvent);
    }
}
