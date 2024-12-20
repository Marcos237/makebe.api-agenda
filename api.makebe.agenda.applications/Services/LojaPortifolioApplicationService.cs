using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.data.interfaces;
using AutoMapper;
using lib.makebe.domain.Interfaces.Services;

namespace api.makebe.agenda.applications.Services
{
    public class LojaPortifolioApplicationService : ILojaPortifolioApplicationService
    {
        private readonly ILojaPortifolioDomainService _lojaPortifolioDomainService;
        private readonly IMapper _mapper;
        private readonly IValidationService<LojaPortifolio> _validationLojaPortifolioService;
        private readonly INotificationContext _notificationContext;
        private readonly ILojaPortifolioImagemApplicationService _lojaPortifolioImagemApplicationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioSessaoDomainService _usuarioSessaoDomainService;
        private readonly IBusEvent _busEvent;
        public LojaPortifolioApplicationService(ILojaPortifolioDomainService lojaPortifolioDomainService, IMapper mapper, IValidationService<LojaPortifolio> validationLojaPortifolioService,
            INotificationContext notificationContext, ILojaPortifolioImagemApplicationService lojaPortifolioImagemApplicationService, 
            IUnitOfWork unitOfWork, IUsuarioSessaoDomainService usuarioSessaoDomainService, IBusEvent busEvent)
        {
            _lojaPortifolioDomainService = lojaPortifolioDomainService;
            _validationLojaPortifolioService = validationLojaPortifolioService;
            _mapper = mapper;
            _notificationContext = notificationContext;
            _lojaPortifolioImagemApplicationService = lojaPortifolioImagemApplicationService;
            _unitOfWork = unitOfWork;
            _usuarioSessaoDomainService = usuarioSessaoDomainService;
            _busEvent = busEvent;
        }
        public async Task<ResponseModel<PaginacaoDTO<LojaPortifolioDTO>>> BuscarLojaPortifolios(PaginacaoDTO<LojaPortifolioDTO> paginacao, string usuarioId)
        {
            var conta = await ContaHelper.BuscarContaPorUsuarioId(usuarioId, _busEvent);
            var paginacaoRetorno = await _lojaPortifolioDomainService.BuscarLojaPortifolios(paginacao, conta.Id.ToString() ?? string.Empty) ?? new PaginacaoDTO<LojaPortifolioDTO>();
            if (paginacaoRetorno != null && !paginacaoRetorno.objetos!.Any())
                _validationLojaPortifolioService.RetornarListaVazia(nameof(LojaPortifolio), BaseConstant.ListaVazia);

            return ResponseModelHelper<PaginacaoDTO<LojaPortifolioDTO>>.RetornarResponseModel(paginacaoRetorno!, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<LojaPortifolioDTO>> BuscarPorId(int id)
        {
            var retorno = await _lojaPortifolioDomainService.BuscarPorId(id);
            retorno.LojaPortifolioImagens = await _lojaPortifolioImagemApplicationService.BuscarImagensPorLojaPortifolioId(id) ?? Enumerable.Empty<LojaPortifolioImagemDTO>();
            if (retorno.Id == 0)
                _validationLojaPortifolioService.RetornarListaVazia(nameof(LojaPortifolio), BaseConstant.ListaVazia);

            return ResponseModelHelper<LojaPortifolioDTO>.RetornarResponseModel(retorno!, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<LojaPortifolioDTO>> Persistir(LojaPortifolioPayload portifolio, string usuarioId)
        {
            var lojaPortifolio = _mapper.Map<LojaPortifolio>(portifolio);
            var arquivos = _mapper.Map<IEnumerable<Arquivo>>(portifolio.LojaPortifolioImagens);
            var arquivoIsvalid = await _lojaPortifolioImagemApplicationService.ValidarArquivos(arquivos);
            var isValidate = await _validationLojaPortifolioService.Validar(lojaPortifolio);
            if (!isValidate || !arquivoIsvalid)
            {
                var lojaErro = _mapper.Map<LojaPortifolioDTO>(lojaPortifolio);
                return ResponseModelHelper<LojaPortifolioDTO>.RetornarResponseModel(lojaErro, _notificationContext.Notifications);
            }
            try
            {
                await _unitOfWork.BeginTransaction();
                var lojaPortifolioRetorno = await _lojaPortifolioDomainService.Salvar(lojaPortifolio);
                if (portifolio.LojaPortifolioImagens!.Any())
                    await _lojaPortifolioImagemApplicationService.SalvarImagens(portifolio.LojaPortifolioImagens!, lojaPortifolioRetorno);
                _unitOfWork.Commit();

                var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuarioId ?? string.Empty);
                await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuarioId ?? string.Empty);

                var retornoPortifolio = await BuscarPorId(lojaPortifolioRetorno);
                return retornoPortifolio;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
        public async Task<bool> Desativar(int id, string usuarioId)
        {
            var portifolioRetorno = await _lojaPortifolioDomainService.Deastivar(id);
            var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuarioId ?? string.Empty);
            await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuarioId ?? string.Empty);
            return portifolioRetorno;
        }
    }
}
