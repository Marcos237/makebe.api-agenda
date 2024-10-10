using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;

namespace api.makebe.agenda.applications.Services
{
    public class TipoLojaApplicationService : ITipoLojaApplicationService
    {
        private readonly ITipoLojaDomainService _tipoLojaDomainService;
        private readonly INotificationContext _notificationContext;
        public TipoLojaApplicationService(ITipoLojaDomainService tipoLojaDomainService, INotificationContext notificationContext)
        {
            _tipoLojaDomainService = tipoLojaDomainService; 
            _notificationContext = notificationContext;
        }
        public async Task<ResponseModel<TipoLoja>> BuscarTodos()
        {
            var retorno = await _tipoLojaDomainService.BuscarTodos() ?? Enumerable.Empty<TipoLoja>();
            if (!retorno.Any())
                _notificationContext.AddNotification(nameof(TipoLoja), BaseConstant.ListaVazia);

            return ResponseModelHelper<TipoLoja>.RetornarResponseModel(retorno, _notificationContext.Notifications);
        }
    }
}
