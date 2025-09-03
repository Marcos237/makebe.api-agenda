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
using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using api.makebe.agenda.infra.data.interfaces;
using AutoMapper;
using ContasEvent;
using lib.makebe.domain.Interfaces.Services;
using PermissoesEvent;
using UsuariosEvent;

namespace api.makebe.agenda.applications.Services.Colaboradores
{
    public class ColaboradorApplicationService : IColaboradorApplicationService
    {
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly INotificationContext _notificationContext;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IColaboradorDomainService _colaboradorDomainService;
        private readonly IContaColaboradorDomainService _usuarioColaboradorDomainService;
        private readonly IUsuarioSessaoDomainService _usuarioSessaoDomainService;
        private readonly IUsuarioEventCrossCuttingService _usuarioEventCrossCuttingService;
        private readonly IPermissaoEventCrossCuttingService _permissaoEventCrossCuttingService;
        public ColaboradorApplicationService(INotificationContext notificationContext, IMapper mapper, IUnitOfWork unitOfWork,
            IColaboradorDomainService colaboradorDomainService, IContaColaboradorDomainService usuarioColaboradorDomainService,
            IUsuarioSessaoDomainService usuarioSessaoDomainService, IContaEventCrossCuttingService contaEventCrossCuttingService,
            IUsuarioEventCrossCuttingService usuarioEventCrossCuttingService, IPermissaoEventCrossCuttingService permissaoEventCrossCuttingService)
        {
            _notificationContext = notificationContext;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _colaboradorDomainService = colaboradorDomainService;
            _usuarioColaboradorDomainService = usuarioColaboradorDomainService;
            _usuarioSessaoDomainService = usuarioSessaoDomainService;
            _usuarioEventCrossCuttingService = usuarioEventCrossCuttingService;
            _permissaoEventCrossCuttingService = permissaoEventCrossCuttingService;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
        }

        public async Task<ResponseModel<PaginacaoDTO<ColaboradorDTO>>> BuscarUsuariosPaginado(PaginacaoDTO<UsuarioDTO> paginacao, string usuario)
        {
            if (paginacao?.objetoPesquisa?.Tipo == (int)TipoUsuario.Cliente)
                return ResponseModelHelper<PaginacaoDTO<ColaboradorDTO>>.RetornarResponseModel(new PaginacaoDTO<ColaboradorDTO>(),
                    _notificationContext.Notifications);

            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuario));
            paginacao!.idsPesquisa = await _colaboradorDomainService.MontarIdsPesquisas(conta?.Id.ToString() ?? string.Empty);
            var usuarioMap = _mapper.Map<PaginacaoEvent<UsuarioEvent>>(paginacao);
            var usuarioPaginadoEvent = new UsuariosPaginadoEvent() { paginacao = usuarioMap };
            var usuarioEvent = await _usuarioEventCrossCuttingService.BuscarPaginado(usuarioPaginadoEvent);
            if (usuarioEvent.NotificationContext!.Any())
            {
                _notificationContext.AddNotifications(usuarioEvent.NotificationContext ?? Enumerable.Empty<Notification>());
                return ResponseModelHelper<PaginacaoDTO<ColaboradorDTO>>.RetornarResponseModel(new PaginacaoDTO<ColaboradorDTO>(), _notificationContext.Notifications);
            }

            var usuarioDTOMap = _mapper.Map<PaginacaoDTO<UsuarioDTO>>(usuarioEvent.paginacao) ?? new PaginacaoDTO<UsuarioDTO>();
            var permissaoEvent = await _permissaoEventCrossCuttingService.BuscarPermissoes(new PermissoesConsultadasEvent());
            var colaboradorFiltado = await _colaboradorDomainService.MontarColaboradoresPaginado(usuarioDTOMap, conta?.Id.ToString() ?? string.Empty, permissaoEvent.Permissoes);
            return ResponseModelHelper<PaginacaoDTO<ColaboradorDTO>>.RetornarResponseModel(colaboradorFiltado, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<ColaboradorDTO>> BuscarUsuarioPorId(string id)
        {
            var idGuid = PropiedadesHelper.ParseGuidOrDefault(id);
            var usuarioConsultadoEvent = new UsuarioConsultadoPorIdEvent() { Id = idGuid };
            var usuarioEvent = await _usuarioEventCrossCuttingService.BuscarUsuarioPorId(usuarioConsultadoEvent);
            var usuarioMap = _mapper.Map<UsuarioDTO>(usuarioEvent.UsuarioConsultadoRetorno) ?? new UsuarioDTO();
            var colaborador = await _colaboradorDomainService.BuscarColaboradorPorIdUsuario(idGuid);
            var colaboradorMap = _mapper.Map<ColaboradorDTO>(usuarioMap);
            colaboradorMap.Status = usuarioEvent?.UsuarioConsultadoRetorno?.Status ?? false;
            colaboradorMap.Id = colaborador.Id;
            return ResponseModelHelper<ColaboradorDTO>.RetornarResponseModel(colaboradorMap, _notificationContext.Notifications);
        }
        public async Task<ResponseModel<ColaboradorDTO>> BuscarColaboladoresPorConta(string usuarioId)
        {
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
            var usuarioConta = new UsuarioContaConsultadoPorContaEvent() { IdConta = conta?.Id ?? Guid.Empty };
            var usuarioEvent = await _contaEventCrossCuttingService.BuscarUsuarioContaPorIdConta(usuarioConta);
            var permissaoId = ConfigHelper.GetValue(BaseConstant.ClientePermissao ?? string.Empty);
            var usuarioContaFiltro = usuarioEvent.UsuariosEvents?.Where(usuario => usuario.PermissaoId != PropiedadesHelper.ParseGuidOrDefault(permissaoId
                ?? string.Empty));
            var usuarioMap = _mapper.Map<IEnumerable<UsuarioDTO>>(usuarioContaFiltro);
            var colaboradorMap = await _colaboradorDomainService.MontarColaboradores(usuarioMap, conta?.Id.ToString() ?? string.Empty);
            return ResponseModelHelper<ColaboradorDTO>.RetornarResponseModel(colaboradorMap, _notificationContext.Notifications);
        }
        public async Task<ResponseModel<ColaboradorDTO>> Persistir(ColaboradorPayload usuarioPayload, string usuario)
        {
            var colaboradorMap = _mapper.Map<Colaborador>(usuarioPayload);
            var registradoEvent = new UsuarioContaRegistradoEvent();
            var contaEvent = new UsuarioContaEvent();
            try
            {
                UsuarioRegistradoEvent usuarioEvent = await SalvarUsuario(usuarioPayload);
                colaboradorMap.UsuarioId = usuarioEvent.UsuarioConsultado.Id;
                colaboradorMap.Status = usuarioPayload.Status;
                if (_notificationContext.Notifications.Any())
                {
                    var usuarioErro = _mapper.Map<UsuarioDTO>(usuarioEvent.UsuarioConsultado);
                    var colaboradorDTO = _mapper.Map<ColaboradorDTO>(usuarioErro);
                    return ResponseModelHelper<ColaboradorDTO>.RetornarResponseModel(colaboradorDTO, _notificationContext.Notifications);
                }
                await SalvarUsuarioConta(usuarioPayload, usuario, colaboradorMap, registradoEvent, contaEvent);

                var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuario ?? string.Empty);
                await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuario ?? string.Empty);
                return await BuscarUsuarioPorId(colaboradorMap.UsuarioId.ToString());
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                var usuarioRollbackEvent = new UsuarioDeletadoEvent() { Id = colaboradorMap.UsuarioId };
                await _usuarioEventCrossCuttingService.DeletarUsuario(usuarioRollbackEvent);
                var usuarioContaRollBackEvent = new UsuarioContaDeletadoEvent() { Id = registradoEvent?.Conta?.Id };
                await _contaEventCrossCuttingService.DeletarContaUsuario(usuarioContaRollBackEvent);
                throw;
            }
        }

        public async Task<UsuarioRegistradoEvent> SalvarUsuario(ColaboradorPayload usuarioPayload)
        {
            if (usuarioPayload.Tipo == (int)TipoUsuario.Cliente)
            {
                usuarioPayload.PermissaoId = ConfigHelper.GetValue(BaseConstant.ClientePermissao ?? string.Empty);
                usuarioPayload.Status = true;
            }

            var usuarioMap = _mapper.Map<UsuarioRegistradoEvent>(usuarioPayload);
            var usuarioEvent = await _usuarioEventCrossCuttingService.SalvarUsuario(usuarioMap);
            if (usuarioEvent.NotificationContext!.Any())
                _notificationContext.AddNotifications(usuarioEvent.NotificationContext ?? Enumerable.Empty<Notification>());

            return usuarioEvent;
        }

        public async Task SalvarUsuarioConta(ColaboradorPayload usuarioPayload, string usuario, Colaborador colaboradorMap,
            UsuarioContaRegistradoEvent registradoEvent, UsuarioContaEvent contaEvent)
        {
            if (usuarioPayload.Id == 0)
            {
                var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuario));
                contaEvent.ContaId = conta?.Id;
                contaEvent.UsuarioId = colaboradorMap.UsuarioId;
                contaEvent.TipoId = PropiedadesHelper.ParseGuidOrDefault(ConfigHelper.GetValue(BaseConstant.TipoContaInicialOPeradorLoja));
                contaEvent.Id = Guid.NewGuid();

                registradoEvent.Conta = contaEvent;
                var usuarioContaEvent = await _contaEventCrossCuttingService.SalvarUsuarioConta(registradoEvent);
                await _unitOfWork.BeginTransaction();
                var colaborador = await _colaboradorDomainService.Salvar(colaboradorMap, usuarioPayload?.UsuarioId ?? string.Empty);
                var usuarioColaboradorMap = new ContaColaborador() { ContaId = conta?.Id, ColaboradorId = colaborador, Status = usuarioPayload!.Status };
                await _usuarioColaboradorDomainService.Salvar(usuarioColaboradorMap, usuarioPayload.Id);
                _unitOfWork.Commit();
            }
        }
    }
}
