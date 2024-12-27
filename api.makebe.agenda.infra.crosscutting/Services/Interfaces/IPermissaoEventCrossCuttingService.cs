using PermissoesEvent;

namespace api.makebe.agenda.infra.crosscutting.Services.Interfaces
{
    public interface IPermissaoEventCrossCuttingService
    {
        Task<PermissoesConsultadasEvent> BuscarPermissoes(PermissoesConsultadasEvent permissoesConsultadasEvent);

    }
}
