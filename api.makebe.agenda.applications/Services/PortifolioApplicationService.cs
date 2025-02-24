using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.applications.Strategys.Interfaces.Portifolios;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.data.interfaces;
using AutoMapper;
using lib.makebe.domain.Interfaces.Services;

namespace api.makebe.agenda.applications.Services
{
    public class PortifolioApplicationService : IPortifolioApplicationService
    {
        private readonly IPortifolioDomainService _portifolioDomainService;
        private readonly IMapper _mapper;
        private readonly IValidationService<Portifolio> _validationLojaPortifolioService;
        private readonly INotificationContext _notificationContext;
        private readonly IPortifolioImagemApplicationService _portifolioImagemApplicationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioSessaoDomainService _usuarioSessaoDomainService;
        private readonly IPortifolioBuscaStrategyContext _portifolioBuscaStrategyContext;
        private readonly IPortifolioPersisteStrategyContext<PortifolioPayload> _portifolioPersisteStrategyContext;
        
        public PortifolioApplicationService(IPortifolioDomainService lojaPortifolioDomainService, IMapper mapper, IValidationService<Portifolio> validationLojaPortifolioService,
            INotificationContext notificationContext, IPortifolioImagemApplicationService lojaPortifolioImagemApplicationService, 
            IUnitOfWork unitOfWork, IUsuarioSessaoDomainService usuarioSessaoDomainService, IPortifolioBuscaStrategyContext portifolioBuscaStrategyContext,
            IPortifolioPersisteStrategyContext<PortifolioPayload> portifolioPersisteStrategyContext)
        {
            _portifolioDomainService = lojaPortifolioDomainService;
            _validationLojaPortifolioService = validationLojaPortifolioService;
            _mapper = mapper;
            _notificationContext = notificationContext;
            _portifolioImagemApplicationService = lojaPortifolioImagemApplicationService;
            _unitOfWork = unitOfWork;
            _usuarioSessaoDomainService = usuarioSessaoDomainService;
            _portifolioBuscaStrategyContext = portifolioBuscaStrategyContext;
            _portifolioPersisteStrategyContext = portifolioPersisteStrategyContext;

        }
        public async Task<ResponseModel<PaginacaoDTO<PortifolioDTO>>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string usuarioId)
        {       
            var paginacaoRetorno = await _portifolioBuscaStrategyContext.Buscar(paginacao, usuarioId);
            if (paginacaoRetorno != null && !paginacaoRetorno.objetos!.Any())
                _validationLojaPortifolioService.RetornarListaVazia(nameof(Portifolio), BaseConstant.ListaVazia);

            return ResponseModelHelper<PaginacaoDTO<PortifolioDTO>>.RetornarResponseModel(paginacaoRetorno!, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<PortifolioDTO>> BuscarPorId(int id, int TipoUsuarioId)
        {
            var retorno = await _portifolioDomainService.BuscarPorId(id);
            retorno.TipoUsuarioId = TipoUsuarioId;
            retorno.PortifolioImagens = await _portifolioImagemApplicationService.BuscarImagensPorLojaPortifolioId(id) ?? Enumerable.Empty<PortifolioImagemDTO>();
            if (retorno.Id == 0)
                _validationLojaPortifolioService.RetornarListaVazia(nameof(Portifolio), BaseConstant.ListaVazia);

            return ResponseModelHelper<PortifolioDTO>.RetornarResponseModel(retorno!, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<PortifolioDTO>> Persistir(PortifolioPayload portifolio, string usuarioId)
        {
            var portifolioItem = _mapper.Map<Portifolio>(portifolio);
            var arquivos = _mapper.Map<IEnumerable<Arquivo>>(portifolio.PortifolioImagens);
            var arquivoIsvalid = await _portifolioImagemApplicationService.ValidarArquivos(arquivos);
            var isValidate = await _validationLojaPortifolioService.Validar(portifolioItem);
            if (!isValidate || !arquivoIsvalid)
            {
                var lojaErro = _mapper.Map<PortifolioDTO>(portifolioItem);
                return ResponseModelHelper<PortifolioDTO>.RetornarResponseModel(lojaErro, _notificationContext.Notifications);
            }
            try
            {
                await _unitOfWork.BeginTransaction();
                var lojaPortifolioRetorno = await _portifolioDomainService.Salvar(portifolioItem);
                if (portifolio.PortifolioImagens!.Any())
                    await _portifolioImagemApplicationService.SalvarImagens(portifolio.PortifolioImagens!, lojaPortifolioRetorno);

                portifolio.Id = lojaPortifolioRetorno;
                await _portifolioPersisteStrategyContext.Salvar(portifolio);
                _unitOfWork.Commit();

                var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuarioId ?? string.Empty);
                await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuarioId ?? string.Empty);

                var retornoPortifolio = await BuscarPorId(lojaPortifolioRetorno, portifolio.TipoUsuarioId);
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
            var portifolioRetorno = await _portifolioDomainService.Deastivar(id);
            var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuarioId ?? string.Empty);
            await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuarioId ?? string.Empty);
            return portifolioRetorno;
        }
    }
}
