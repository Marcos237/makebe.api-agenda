using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Enums;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using api.makebe.agenda.infra.data.interfaces;
using AutoMapper;
using lib.makebe.domain.Interfaces.Services;

namespace api.makebe.agenda.applications.Services.Agendas
{
    public class AgendaApplicationService : IAgendaApplicationService
    {
        private readonly IAgendaDomainService _agendaDomainService;
        private readonly IValidationService<Agenda> _validation;
        private readonly IValidationService<AgendaLoja> _validationLoja;
        private readonly IValidationService<AgendaColaborador> _validationColaborador;
        private readonly INotificationContext _notificationContext;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly IUsuarioSessaoDomainService _usuarioSessaoDomainService;
        private readonly IAgendaContextDomainService<AgendaLoja> _contextFactoryLoja;
        private readonly IAgendaContextDomainService<AgendaColaborador> _contextFactoryColaborador;
        public AgendaApplicationService(
            IAgendaContextDomainService<AgendaLoja> contextFactoryLoja,
            IAgendaContextDomainService<AgendaColaborador> contextFactoryColaborador,
            IAgendaDomainService agendaDomainService,
            IValidationService<Agenda> validation,
            INotificationContext notificationContext, IMapper mapper, IUnitOfWork unitOfWork,
            IContaEventCrossCuttingService contaEventCrossCuttingService, IUsuarioSessaoDomainService usuarioSessaoDomainService,
            IValidationService<AgendaLoja> validationLoja, IValidationService<AgendaColaborador> validationColaborador)
        {
            _agendaDomainService = agendaDomainService;
            _validation = validation;
            _notificationContext = notificationContext;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _usuarioSessaoDomainService = usuarioSessaoDomainService;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _contextFactoryLoja = contextFactoryLoja;
            _contextFactoryColaborador = contextFactoryColaborador;
            _validationLoja = validationLoja;
            _validationColaborador = validationColaborador;
        }

        public async Task<ResponseModel<PaginacaoDTO<AgendaDTO>>> BuscarTodosPaginado(PaginacaoDTO<AgendaPayload> paginacao, string usuarioId)
        {
            var usuario = PropiedadesHelper.ParseGuidOrDefault(usuarioId);
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(usuario);
            var paginacaoDTO = _mapper.Map<PaginacaoDTO<AgendaDTO>>(paginacao);

            var response = paginacao?.objetoPesquisa?.Tipo == (int)TipoUsuario.Loja ?
                await _contextFactoryLoja.BuscarPaginado(paginacaoDTO, conta.Id.ToString() ?? string.Empty) :
                 await _contextFactoryColaborador.BuscarPaginado(paginacaoDTO, conta.Id.ToString() ?? string.Empty);

            if (response != null && !response.objetos!.Any())
                _validation.RetornarListaVazia(nameof(Loja), BaseConstant.ListaVazia);

            return ResponseModelHelper<PaginacaoDTO<AgendaDTO>>.RetornarResponseModel(response!, _notificationContext.Notifications);
        }
        public async Task<ResponseModel<AgendaDTO>> BuscarPorId(int id, int tipo)
        {
            var response = tipo == (int)TipoUsuario.Loja ?
                await _contextFactoryLoja.BuscarPorId(id) :
                 await _contextFactoryColaborador.BuscarPorId(id);
            if (response.Id == 0)
                _validation.RetornarListaVazia(BaseConstant.ListaVazia, nameof(Loja));

            return ResponseModelHelper<AgendaDTO>.RetornarResponseModel(response, _notificationContext.Notifications);
        }
        public async Task<ResponseModel<AgendaDTO>> Persitir(AgendaPayload payload, string usuarioId)
        {
            var agenda = _mapper.Map<Agenda>(payload);
            var agendaColaborador = _mapper.Map<AgendaColaborador>(payload);
            var isValidateItem = false;
            var agendaLoja = _mapper.Map<AgendaLoja>(payload);
            if (payload?.Tipo == (int)TipoUsuario.Loja)
                isValidateItem = await _validationLoja.Validar(agendaLoja);
            if (payload?.Tipo == (int)TipoUsuario.Colaborador)
                isValidateItem = await _validationColaborador.Validar(agendaColaborador);


            var isValidate = await _validation.Validar(agenda);
            if (!isValidate && !isValidateItem)
            {
                var agendaResponseErro = _mapper.Map<AgendaDTO>(payload);
                return ResponseModelHelper<AgendaDTO>.RetornarResponseModel(agendaResponseErro, _notificationContext.Notifications);
            }
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
            try
            {
                await _unitOfWork.BeginTransaction();
                var agendaRetorno = await _agendaDomainService.Persitir(agenda);

                if (payload?.Tipo == (int)TipoUsuario.Loja)
                {

                    agendaLoja.IdAgenda = agendaRetorno;
                    await _contextFactoryLoja.Persistir(agendaLoja);
                }
                if (payload?.Tipo == (int)TipoUsuario.Colaborador)
                {
                    agendaColaborador.IdAgenda = agendaRetorno;
                    await _contextFactoryColaborador.Persistir(agendaColaborador);
                }

                _unitOfWork.Commit();
                var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuarioId ?? string.Empty);
                await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuarioId ?? string.Empty);

                var agendaResponse = await BuscarPorId(agendaRetorno, payload!.Tipo);
                return agendaResponse;
            }
            catch (Exception)
            {

                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> Desativar(int id, string usuarioId)
        {
            var retorno = await _agendaDomainService.Desativar(id);
            var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuarioId ?? string.Empty);
            await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuarioId ?? string.Empty);
            return retorno;
        }
    }
}
