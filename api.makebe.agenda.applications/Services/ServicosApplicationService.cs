using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;

namespace api.makebe.agenda.applications.Services
{
    public class ServicosApplicationService : IServicoApplicationService
    {
        private readonly IServicosDomainService _servicosDomainService;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly INotificationContext _notificationContext; 
        public ServicosApplicationService(IServicosDomainService servicosDomainService, IContaEventCrossCuttingService contaEventCrossCuttingService, 
            INotificationContext notificationContext)
        {
            _servicosDomainService = servicosDomainService;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _notificationContext = notificationContext;
        }
        public async Task<ResponseModel<Servicos>> BuscarServicos(string usuarioId)
        {
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
            var retorno =  await _servicosDomainService.BuscarServicos(conta?.Id.ToString() ?? string.Empty);
            return ResponseModelHelper<Servicos>.RetornarResponseModel(retorno, _notificationContext.Notifications);
        }
    }
}
