using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using api.makebe.agenda.infra.crosscutting.Notifications;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.data.interfaces;
using api.makebesession.infra.crosscutting.Entidades;
using api.makebesession.infra.crosscutting.Events.Conta;
using api.makebesession.infra.crosscutting.Events.Contas;
using api.makebesession.infra.crosscutting.Events.Permissoes;
using api.makebesession.infra.crosscutting.Events.Usuarios;
using AutoMapper;
using lib.makebe.domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace api.makebe.agenda.applications.Services
{
    public class ColaboradorApplicationService : IColaboradorApplicationService
    {
        private readonly IBusEvent _busEvent;
        private readonly INotificationContext _notificationContext;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IColaboradorDomainService _colaboradorDomainService;
        private readonly IContaColaboradorDomainService _usuarioColaboradorDomainService;
        private readonly IUsuarioSessaoDomainService _usuarioSessaoDomainService;
        private readonly IConfiguration _configuration;
        public ColaboradorApplicationService(IBusEvent busEvent, INotificationContext notificationContext, IMapper mapper, IUnitOfWork unitOfWork,
            IColaboradorDomainService colaboradorDomainService, IContaColaboradorDomainService usuarioColaboradorDomainService, IConfiguration configuration,
            IUsuarioSessaoDomainService usuarioSessaoDomainService)
        {
            _busEvent = busEvent;
            _notificationContext = notificationContext;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _colaboradorDomainService = colaboradorDomainService;
            _usuarioColaboradorDomainService = usuarioColaboradorDomainService;
            _usuarioSessaoDomainService = usuarioSessaoDomainService;
            _configuration = configuration;
        }

        public async Task<ResponseModel<PaginacaoDTO<ColaboradorDTO>>> BuscarUsuariosPaginado(PaginacaoDTO<UsuarioDTO> paginacao, string usuario)
        {
            var contaConsultadoEvent = new ContaConsultadoPorIdEvent() { Id = PropiedadesHelper.ParseGuidOrDefault(usuario) };
            var contaRetorno = await _busEvent.RequestAsync<ContaConsultadoPorIdEvent, ContaConsultadoPorIdEvent>(contaConsultadoEvent, TimeSpan.FromSeconds(15));

            paginacao.idsPesquisa = await _colaboradorDomainService.MontarIdsPesquisas(contaRetorno?.ContaEvent?.Id?.ToString() ?? string.Empty);
            var usuarioMap = _mapper.Map<PaginacaoEvent<UsuarioEvent>>(paginacao) ?? new PaginacaoEvent<UsuarioEvent>();
            var usuarioPaginadoEvent = new UsuariosPaginadoEvent() { paginacao = usuarioMap };
            var usuarioEvent = await _busEvent.RequestAsync<UsuariosPaginadoEvent, UsuariosPaginadoEvent>(usuarioPaginadoEvent, TimeSpan.FromSeconds(15));
            if (usuarioEvent.NotificationContext!.Any())
            {
                _notificationContext.AddNotifications(usuarioEvent.NotificationContext ?? Enumerable.Empty<Notification>());
                return ResponseModelHelper<PaginacaoDTO<ColaboradorDTO>>.RetornarResponseModel(new PaginacaoDTO<ColaboradorDTO>(), _notificationContext.Notifications);
            }

            var usuarioDTOMap = _mapper.Map<PaginacaoDTO<UsuarioDTO>>(usuarioEvent.paginacao) ?? new PaginacaoDTO<UsuarioDTO>();
            var permissaoEvent = await _busEvent.RequestAsync<PermissoesConsultadasEvent, PermissoesConsultadasEvent>(new PermissoesConsultadasEvent(), TimeSpan.FromSeconds(15));
            var colaboradorFiltado = await _colaboradorDomainService.MontarColaboradores(usuarioDTOMap, contaRetorno?.ContaEvent?.Id?.ToString() ?? string.Empty, permissaoEvent.Permissoes);
            return ResponseModelHelper<PaginacaoDTO<ColaboradorDTO>>.RetornarResponseModel(colaboradorFiltado, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<ColaboradorDTO>> BuscarUsuarioPorId(string id)
        {
            var idGuid = PropiedadesHelper.ParseGuidOrDefault(id);
            var usuarioConsultadoEvent = new UsuarioConsultadoPorIdEvent() { Id = idGuid };
            var usuarioEvent = await _busEvent.RequestAsync<UsuarioConsultadoPorIdEvent, UsuarioConsultadoPorIdEvent>(usuarioConsultadoEvent, TimeSpan.FromSeconds(15));
            var usuarioMap = _mapper.Map<UsuarioDTO>(usuarioEvent.UsuarioConsultadoRetorno) ?? new UsuarioDTO();
            var colaborador = await _colaboradorDomainService.BuscarColaboradorPorIdUsuario(idGuid);
            var colaboradorMap = _mapper.Map<ColaboradorDTO>(usuarioMap);
            colaboradorMap.Status = usuarioEvent?.UsuarioConsultadoRetorno?.Status ?? false;
            colaboradorMap.Id = colaborador.Id;
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
                if (_notificationContext.Notifications.Any())
                {
                    var usuarioErro = _mapper.Map<UsuarioDTO>(usuarioEvent.UsuarioConsultado);
                    var colaboradorDTO = _mapper.Map<ColaboradorDTO>(usuarioErro);
                    return ResponseModelHelper<ColaboradorDTO>.RetornarResponseModel(colaboradorDTO, _notificationContext.Notifications);
                }

                colaboradorMap.UsuarioId = usuarioEvent.UsuarioConsultado.Id;
                await SalvarUsuarioConta(usuarioPayload, usuario, colaboradorMap, registradoEvent, contaEvent);

                await _unitOfWork.BeginTransaction();
                var colaborador = await _colaboradorDomainService.Salvar(colaboradorMap);
                var usuarioColaboradorMap = new ContaColaborador() { ContaId = contaEvent.ContaId, ColaboradorId = colaborador };
                await _usuarioColaboradorDomainService.Salvar(usuarioColaboradorMap);
                _unitOfWork.Commit();
                var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuario ?? string.Empty);
                await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuario ?? string.Empty);
                return await BuscarUsuarioPorId(colaboradorMap.UsuarioId.ToString());
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                var usuarioRollbackEvent = new UsuarioDeletadoEvent() { Id = colaboradorMap.UsuarioId };
                await _busEvent.PublishAsync(usuarioRollbackEvent);
                var usuarioContaRollBackEvent = new UsuarioContaDeletadoEvent() { Id = registradoEvent?.Conta?.Id };
                await _busEvent.PublishAsync(usuarioContaRollBackEvent);
                throw;
            }

        }

        public async Task<UsuarioRegistradoEvent> SalvarUsuario(ColaboradorPayload usuarioPayload)
        {
            var usuarioMap = _mapper.Map<UsuarioRegistradoEvent>(usuarioPayload);
            var usuarioEvent = await _busEvent.RequestAsync<UsuarioRegistradoEvent, UsuarioRegistradoEvent>(usuarioMap, TimeSpan.FromSeconds(15));
            if (usuarioEvent.NotificationContext!.Any())
                _notificationContext.AddNotifications(usuarioEvent.NotificationContext ?? Enumerable.Empty<Notification>());

            return usuarioEvent;
        }

        public async Task SalvarUsuarioConta(ColaboradorPayload usuarioPayload, string usuario, Colaborador colaboradorMap, UsuarioContaRegistradoEvent registradoEvent,
            UsuarioContaEvent contaEvent)
        {
            if (usuarioPayload.Id == 0)
            {
                var contaConsultadoEvent = new ContaConsultadoPorIdEvent() { Id = PropiedadesHelper.ParseGuidOrDefault(usuario) };
                var contaRetorno = await _busEvent.RequestAsync<ContaConsultadoPorIdEvent, ContaConsultadoPorIdEvent>(contaConsultadoEvent, TimeSpan.FromSeconds(15));
                contaEvent.ContaId = contaRetorno?.ContaEvent?.Id;
                contaEvent.UsuarioId = colaboradorMap.UsuarioId;
                contaEvent.TipoId = PropiedadesHelper.ParseGuidOrDefault(_configuration[BaseConstant.TipoContaInicialOPeradorLoja]);
                contaEvent.Id = Guid.NewGuid();
                registradoEvent.Conta = contaEvent;
                var usuarioContaEvent = await _busEvent.RequestAsync<UsuarioContaRegistradoEvent, UsuarioContaRegistradoEvent>(registradoEvent, TimeSpan.FromSeconds(15));
            }
        }
    }
}
