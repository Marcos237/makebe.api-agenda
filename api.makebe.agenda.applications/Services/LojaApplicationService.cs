using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.domain.Services;
using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.data.interfaces;
using api.makebesession.infra.crosscutting.Events.Contas;
using AutoMapper;
using lib.makebe.domain.Interfaces.Services;

namespace api.makebe.agenda.applications.Services
{
    public class LojaApplicationService : ILojaApplicationService
    {
        private readonly IValidationService<Loja> _validationService;
        private readonly IContaLojaDomainService _contaLojaDomainService;
        private readonly ILojaDomainService _lojaDomainService;
        private readonly INotificationContext _notificationContext;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioSessaoDomainService _usuarioSessaoDomainService;
        private readonly IBusEvent _busEvent;

        public LojaApplicationService(IValidationService<Loja> validationService, IContaLojaDomainService usarioLojaDomainService, ILojaDomainService lojaDomainService,
            INotificationContext notificationContext, IMapper mapper, IUnitOfWork unitOfWork, IBusEvent busEvent,
            IUsuarioSessaoDomainService usuarioSessaoDomainService)
        {
            _lojaDomainService = lojaDomainService;
            _validationService = validationService;
            _contaLojaDomainService = usarioLojaDomainService;
            _notificationContext = notificationContext;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _usuarioSessaoDomainService = usuarioSessaoDomainService;
            _busEvent = busEvent;
        }
        public async Task<ResponseModel<LojaDTO>> BuscarTodos(string usuarioId)
        {
            var lojas = await _lojaDomainService.BuscarTodos(usuarioId);
            if (!lojas.Any())
                _validationService.RetornarListaVazia(nameof(Loja), BaseConstant.ListaVazia);

            return ResponseModelHelper<LojaDTO>.RetornarResponseModel(lojas, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<PaginacaoDTO<LojaResponse>>> BuscarTodosPaginado(PaginacaoDTO<LojaPayload> lojaPayload, string usuarioId)
        {
            var conta = await ContaHelper.BuscarContaPorUsuarioId(usuarioId, _busEvent);
            var paginacaoDTO = _mapper.Map<PaginacaoDTO<LojaDTO>>(lojaPayload) ?? new PaginacaoDTO<LojaDTO>();
            var result = await _lojaDomainService.BuscarTodosPaginado(paginacaoDTO, conta.Id.ToString() ?? string.Empty) ?? new PaginacaoDTO<LojaDTO>();
            if (result != null && !result.objetos!.Any())
                _validationService.RetornarListaVazia(nameof(Loja), BaseConstant.ListaVazia);

            var lojaResponse = _mapper.Map<PaginacaoDTO<LojaResponse>>(result);
            return ResponseModelHelper<PaginacaoDTO<LojaResponse>>.RetornarResponseModel(lojaResponse, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<LojaResponse>> BuscarPorId(int id)
        {
            var result = await _lojaDomainService.BuscarPorId(id);
            if (result.Id == 0)
                _validationService.RetornarListaVazia(BaseConstant.ListaVazia, nameof(Loja));

            var lojaRetorno = _mapper.Map<LojaResponse>(result);
            return ResponseModelHelper<LojaResponse>.RetornarResponseModel(lojaRetorno, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<LojaResponse>> Persitir(LojaPayload lojaPayload, string usuarioId)
        {
            var loja = _mapper.Map<Loja>(lojaPayload);
            var isValidate = await _validationService.Validar(loja);
            if (!isValidate)
            {
                var lojaResponseErro = _mapper.Map<LojaResponse>(loja);
                return ResponseModelHelper<LojaResponse>.RetornarResponseModel(lojaResponseErro, _notificationContext.Notifications);
            }
            var conta = await ContaHelper.BuscarContaPorUsuarioId(usuarioId, _busEvent);
            try
            {
                await _unitOfWork.BeginTransaction();
                var lojaRetorno = await _lojaDomainService.Persitir(loja);
                var contaLoja = new ContaLoja() { LojaId = lojaRetorno, ContaId = conta.Id};

                if (lojaPayload.Id == 0)
                    await _contaLojaDomainService.Salvar(contaLoja);
                _unitOfWork.Commit();
                var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuarioId ?? string.Empty);
                await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuarioId ?? string.Empty);

                var lojaResponse = await BuscarPorId(lojaRetorno);
                return lojaResponse;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> Desativar(int id, string usuarioId)
        {
            var retorno = await _lojaDomainService.Desativar(id);
            var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuarioId ?? string.Empty);
            await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuarioId ?? string.Empty);
            return retorno;
        }
    }
}
