using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using ContasEvent;

namespace api.makebe.agenda.applications.Services
{
    public class AgendamentoApplicationService : IAgendamentoApplicationService
    {
        private readonly IAgendamentoDomainService _agendamentoDomainService;
        private readonly INotificationContext _notificationContext;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly IUsuarioClienteConsultadosCrosCuttingService _consultadosCrosCuttingService;

        public AgendamentoApplicationService(IAgendamentoDomainService agendamentoDomainService, INotificationContext notificationContext, IContaEventCrossCuttingService contaEventCrossCuttingService,
            IUsuarioClienteConsultadosCrosCuttingService consultadosCrosCuttingService)
        {
            _agendamentoDomainService = agendamentoDomainService;
            _notificationContext = notificationContext;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _consultadosCrosCuttingService = consultadosCrosCuttingService;
        }
        public async Task<ResponseModel<PaginacaoDTO<AgendamentoDTO>>> BuscarAgendamentoPaginado(PaginacaoDTO<AgendamentoDTO> paginacao, string usuario)
        {
            var clientes = await _consultadosCrosCuttingService.BuscarUsuarioClientes();
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuario));
            var contaEvent = new UsuarioContaConsultadoPorContaEvent() { IdConta = PropiedadesHelper.ParseGuidOrDefault(conta.Id.ToString()) };
            var colaboradores = await _contaEventCrossCuttingService.BuscarUsuarioContaPorIdConta(contaEvent);
            var response = await _agendamentoDomainService.MontarAgendamento(paginacao, conta?.Id?.ToString() ?? string.Empty, clientes, colaboradores.UsuariosEvents);
            var responseFilter = _agendamentoDomainService.Filtrar(response);

            return ResponseModelHelper<PaginacaoDTO<AgendamentoDTO>>.RetornarResponseModel(new PaginacaoDTO<AgendamentoDTO>(), _notificationContext.Notifications);
        }

        public Task<ResponseModel<AgendamentoDTO>> BuscarAgendamentoPorId(string id)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseModel<AgendamentoDTO>> Persistir(ColaboradorPayload usuarioPayload, string usuario)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<AgendamentoDTO>> Desativar(int id)
        {
            throw new NotImplementedException();
        }

    }
}
