using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using api.makebe.agenda.infra.data.interfaces;
using AutoMapper;
using lib.makebe.domain.Interfaces.Services;

namespace api.makebe.agenda.applications.Services
{
    public class ServicosApplicationService : IServicoApplicationService
    {
        private readonly IServicosDomainService _servicosDomainService;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly INotificationContext _notificationContext;
        private readonly IValidationService<Servicos> _validationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IContaServicoDomainService _contaServicoDomainService;
        private readonly IUsuarioSessaoDomainService _usuarioSessaoDomainService;
        private readonly IMapper _mapper;
        public ServicosApplicationService(IServicosDomainService servicosDomainService, IContaEventCrossCuttingService contaEventCrossCuttingService, 
            INotificationContext notificationContext, IValidationService<Servicos> validationService, IUnitOfWork unitOfWork, IContaServicoDomainService contaServicoDomainService
           ,IUsuarioSessaoDomainService usuarioSessaoDomainService, IMapper mapper)
        {
            _servicosDomainService = servicosDomainService;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _notificationContext = notificationContext;
            _validationService = validationService;
            _unitOfWork = unitOfWork;
            _contaServicoDomainService = contaServicoDomainService;
            _usuarioSessaoDomainService = usuarioSessaoDomainService;   
            _mapper = mapper;
        }

        public async Task<ResponseModel<Servicos>> BuscarServicos(string usuarioId)
        {
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
            var retorno = await _servicosDomainService.BuscarServicos(conta?.Id.ToString() ?? string.Empty);
            return ResponseModelHelper<Servicos>.RetornarResponseModel(retorno, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<PaginacaoDTO<ServicoDTO>>> BuscarTodosPaginado(PaginacaoDTO<ServicoDTO> paginacaoDTO, string usuarioId)
        {
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
            var response = await _servicosDomainService.BuscarTodosPaginado(paginacaoDTO, conta.Id.ToString() ?? string.Empty) ?? new PaginacaoDTO<ServicoDTO>();
            if (response != null && !response.objetos!.Any())
                _validationService.RetornarListaVazia(nameof(Servicos), BaseConstant.ListaVazia);

            return ResponseModelHelper<PaginacaoDTO<ServicoDTO>>.RetornarResponseModel(response!, _notificationContext.Notifications);
        }
        public async Task<ResponseModel<ServicoDTO>> BuscarPorId(int id)
        {
            var response = await _servicosDomainService.BuscarPorId(id);
            if (response.Id == 0)
                _validationService.RetornarListaVazia(BaseConstant.ListaVazia, nameof(ServicoDTO));

            var responseMap = _mapper.Map<ServicoDTO>(response);
            return ResponseModelHelper<ServicoDTO>.RetornarResponseModel(responseMap, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<ServicoDTO>> Persitir(ServicoDTO item, string usuarioId)
        {
            var servicoMap = _mapper.Map<Servicos>(item);
            var isValidate = await _validationService.Validar(servicoMap);
            if (!isValidate)
                return ResponseModelHelper<ServicoDTO>.RetornarResponseModel(item, _notificationContext.Notifications);

            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
            try
            {
                await _unitOfWork.BeginTransaction();
                var servicoRetorno = await _servicosDomainService.Persitir(servicoMap);
                var contaServico = new ContaServico() { ServicoId = servicoRetorno, ContaId = conta?.Id.ToString() };

                if (item.Id == 0)
                    await _contaServicoDomainService.Salvar(contaServico, item.Id);
                _unitOfWork.Commit();
                var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuarioId ?? string.Empty);
                await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuarioId ?? string.Empty);

                var servicoResponse = await BuscarPorId(servicoRetorno);
                return servicoResponse;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
        public async  Task<bool> Desativar(int id, string usuarioId)
        {
            var retorno = await _servicosDomainService.Desativar(id);
            var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuarioId ?? string.Empty);
            await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuarioId ?? string.Empty);
            return retorno;
        }
    }
}
