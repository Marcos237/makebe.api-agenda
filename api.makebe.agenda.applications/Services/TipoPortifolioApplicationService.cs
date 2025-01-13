using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;

namespace api.makebe.agenda.applications.Services
{
    public class TipoPortifolioApplicationService : ITipoPortifolioApplicationService
    {
        private ITipoPortifolioDomainService _tipoPortifolioDomainService;
        private readonly INotificationContext _notificationContext;
        public TipoPortifolioApplicationService(ITipoPortifolioDomainService tipoPortifolioDomainService, INotificationContext notificationContext)
        {
            _tipoPortifolioDomainService = tipoPortifolioDomainService;
            _notificationContext = notificationContext;
        }
        public async Task<ResponseModel<TipoPortifolioDTO>> BuscarPorTipoUsuarioPortifolioId(int tipoPortifolioId)
        {
            var request = await _tipoPortifolioDomainService.BuscarPorTipoUsuarioPortifolioId(tipoPortifolioId);
            return ResponseModelHelper<TipoPortifolioDTO>.RetornarResponseModel(request, _notificationContext.Notifications);
        }
    }
}
