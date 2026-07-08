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

namespace api.makebe.agenda.applications.Services.Servicos
{
    public class ServicosApplicationService : IServicoApplicationService
    {
        private readonly IServicosDomainService _servicosDomainService;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly INotificationContext _notificationContext;
        private readonly IValidationService<Servico> _validationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IContaServicoDomainService _contaServicoDomainService;
        private readonly ICategoriaDomainService _categoriaDomainService;
        private readonly ICategoriaItemDomainService _categoriaItemDomainService;
        private readonly IUsuarioSessaoDomainService _usuarioSessaoDomainService;
        private readonly IMapper _mapper;
        public ServicosApplicationService(IServicosDomainService servicosDomainService, IContaEventCrossCuttingService contaEventCrossCuttingService,
            INotificationContext notificationContext, IValidationService<Servico> validationService, IUnitOfWork unitOfWork, IContaServicoDomainService contaServicoDomainService
           , ICategoriaDomainService categoriaDomainService, ICategoriaItemDomainService categoriaItemDomainService, IUsuarioSessaoDomainService usuarioSessaoDomainService, IMapper mapper)
        {
            _servicosDomainService = servicosDomainService;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _notificationContext = notificationContext;
            _validationService = validationService;
            _unitOfWork = unitOfWork;
            _contaServicoDomainService = contaServicoDomainService;
            _categoriaDomainService = categoriaDomainService;
            _categoriaItemDomainService = categoriaItemDomainService;
            _usuarioSessaoDomainService = usuarioSessaoDomainService;
            _mapper = mapper;
        }

        public async Task<ResponseModel<Servico>> BuscarServicos(string usuarioId)
        {
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
            var retorno = await _servicosDomainService.BuscarServicos(conta?.Id.ToString() ?? string.Empty);
            return ResponseModelHelper<Servico>.RetornarResponseModel(retorno, _notificationContext.Notifications);
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
            var categoria = (await _categoriaDomainService.BuscarPorServico(id)).FirstOrDefault();
            if (categoria != null)
            {
                var categoriaItem = (await _categoriaItemDomainService.BuscarTodosAtivos())
                    .FirstOrDefault(x => x.Descricao == categoria.Descricao);
                responseMap.CategoriaItemId = categoriaItem?.Id ?? 0;
            }

            responseMap.CategoriaItens = await _categoriaItemDomainService.BuscarTodosAtivos();
            return ResponseModelHelper<ServicoDTO>.RetornarResponseModel(responseMap, _notificationContext.Notifications);
        }
        public async Task<ResponseModel<CategoriaItem>> BuscarCategorias()
        {
            var response = await _categoriaItemDomainService.BuscarTodosAtivos() ?? Enumerable.Empty<CategoriaItem>();
            if (!response.Any())
                _notificationContext.AddNotification(nameof(CategoriaItem), BaseConstant.ListaVazia);

            return ResponseModelHelper<CategoriaItem>.RetornarResponseModel(response, _notificationContext.Notifications);
        }
        public async Task<ResponseModel<ServicoDTO>> BuscarServicosPorColaboradoId(int id)
        {
            var response = await _servicosDomainService.BuscarServicosPorColaboradoId(id);
            if (response.Any() == false)
                _validationService.RetornarListaVazia(BaseConstant.ListaVazia, nameof(ServicoDTO));

            var responseMap = _mapper.Map<IEnumerable<ServicoDTO>>(response);
            return ResponseModelHelper<ServicoDTO>.RetornarResponseModel(responseMap, _notificationContext.Notifications);
        }
        public async Task<ResponseModel<ServicoDTO>> Persitir(ServicoDTO item, string usuarioId)
        {
            var servicoMap = _mapper.Map<Servico>(item);
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

                await PersitirCategoria(item, servicoRetorno);
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
        public async Task<bool> Desativar(int id, string usuarioId)
        {
            var retorno = await _servicosDomainService.Desativar(id);
            var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuarioId ?? string.Empty);
            await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuarioId ?? string.Empty);
            return retorno;
        }

        private async Task PersitirCategoria(ServicoDTO item, int servicoId)
        {
            if (item.CategoriaItemId <= 0)
                return;

            if (item.Id > 0)
                await _categoriaDomainService.DesativarPorServico(servicoId);

            var categoriaItem = await _categoriaItemDomainService.BuscarPorId(item.CategoriaItemId);
            if (categoriaItem == null)
                return;

            await _categoriaDomainService.Salvar(new Categoria
            {
                CategoriaItemId = item.CategoriaItemId,
                ServicoId = servicoId,
                Descricao = categoriaItem.Descricao,
                DataCadastro = DateTime.Now,
                Ativo = true
            });
        }
    }
}
