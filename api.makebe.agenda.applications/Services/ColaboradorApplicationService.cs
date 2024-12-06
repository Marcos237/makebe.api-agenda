using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using api.makebe.agenda.infra.crosscutting.Notifications;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.data.interfaces;
using api.makebesession.infra.crosscutting.Entidades;
using api.makebesession.infra.crosscutting.Events.Usuarios;
using AutoMapper;
using lib.makebe.domain.Interfaces.Services;

namespace api.makebe.agenda.applications.Services
{
    public class ColaboradorApplicationService : IColaboradorApplicationService
    {
        private readonly IBusEvent _busEvent;
        private readonly INotificationContext _notificationContext;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IColaboradorDomainService _colaboradorDomainService;
        private readonly ILojaColaboradorDomainService _lojaColaboradorDomainService;
        private readonly IUsuarioSessaoDomainService _usuarioSessaoDomainService;
        public ColaboradorApplicationService(IBusEvent busEvent, INotificationContext notificationContext, IMapper mapper, IUnitOfWork unitOfWork,
            IColaboradorDomainService colaboradorDomainService, ILojaColaboradorDomainService lojaColaboradorDomainService,
            IUsuarioSessaoDomainService usuarioSessaoDomainService)
        {
            _busEvent = busEvent;
            _notificationContext = notificationContext;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _colaboradorDomainService = colaboradorDomainService;
            _lojaColaboradorDomainService = lojaColaboradorDomainService;
            _usuarioSessaoDomainService = usuarioSessaoDomainService;
        }

        public async Task<ResponseModel<PaginacaoDTO<ColaboradorDTO>>> BuscarUsuariosPaginado(PaginacaoDTO<UsuarioDTO> paginacao, string usuario)
        {
            var usuarioMap = _mapper.Map<PaginacaoEvent<UsuarioEvent>>(paginacao) ?? new PaginacaoEvent<UsuarioEvent>();
            var usuarioPaginadoEvent = new UsuariosPaginadoEvent() { paginacao = usuarioMap };
            var usuarioEvent = await _busEvent.RequestAsync<UsuariosPaginadoEvent, UsuariosPaginadoEvent>(usuarioPaginadoEvent, TimeSpan.FromSeconds(15));
            if (usuarioEvent.NotificationContext!.Any())
            {
                _notificationContext.AddNotifications(usuarioEvent.NotificationContext ?? Enumerable.Empty<Notification>());
                return ResponseModelHelper<PaginacaoDTO<ColaboradorDTO>>.RetornarResponseModel(new PaginacaoDTO<ColaboradorDTO>(), _notificationContext.Notifications);
            }

            var usuarioDTOMap = _mapper.Map<PaginacaoDTO<UsuarioDTO>>(usuarioEvent.paginacao) ?? new PaginacaoDTO<UsuarioDTO>();
            var colaboradorFiltado = await _colaboradorDomainService.MontarColaboradores(usuarioDTOMap, usuario);
            return ResponseModelHelper<PaginacaoDTO<ColaboradorDTO>>.RetornarResponseModel(colaboradorFiltado, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<ColaboradorDTO>> BuscarUsuarioPorId(Guid id)
        {
            var usuarioConsultadoEvent = new UsuarioConsultadoPorIdEvent() { Id = id };
            var usuarioEvent = await _busEvent.RequestAsync<UsuarioConsultadoPorIdEvent, UsuarioConsultadoPorIdEvent>(usuarioConsultadoEvent, TimeSpan.FromSeconds(15));
            var usuarioMap = _mapper.Map<UsuarioDTO>(usuarioEvent) ?? new UsuarioDTO();
            var colaborador = await _colaboradorDomainService.BuscarColaboradorPorIdUsuario(id);
            colaborador.Usuario = usuarioMap;
            return ResponseModelHelper<ColaboradorDTO>.RetornarResponseModel(colaborador, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<ColaboradorDTO>> Persistir(ColaboradorPayload usuarioPayload, string usuario)
        {
            var usuarioMap = _mapper.Map<UsuarioRegistradoEvent>(usuarioPayload);
            var usuarioEvent = await _busEvent.RequestAsync<UsuarioRegistradoEvent, UsuarioRegistradoEvent>(usuarioMap, TimeSpan.FromSeconds(15));
            if (usuarioEvent.NotificationContext!.Any())
            {
                var colaboradorErro = _mapper.Map<ColaboradorDTO>(usuarioEvent.UsuarioConsultado);
                _notificationContext.AddNotifications(usuarioEvent.NotificationContext ?? Enumerable.Empty<Notification>());
                return ResponseModelHelper<ColaboradorDTO>.RetornarResponseModel(colaboradorErro, _notificationContext.Notifications);
            }
            var colaboradorMap = _mapper.Map<Colaborador>(usuarioPayload);
            colaboradorMap.UsuarioId = usuarioEvent.UsuarioConsultado.Id;
            try
            {
                await _unitOfWork.BeginTransaction();
                var colaborador = await _colaboradorDomainService.Salvar(colaboradorMap);
                var lojaColaboradorMap = _mapper.Map<LojaColaborador>(usuarioPayload);
                await _lojaColaboradorDomainService.Persistir(lojaColaboradorMap);
                _unitOfWork.Commit();
                var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuario ?? string.Empty);
                await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuario ?? string.Empty);
                return await BuscarUsuarioPorId(colaboradorMap.UsuarioId);
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                var usuarioRollbackEvent = new UsuarioDeletadoEvent() { Id = colaboradorMap.UsuarioId };
                await _busEvent.PublishAsync(usuarioRollbackEvent);
                throw;
            }
        }

        public async Task<bool> Desativar(int id, string usuarioId)
        {
            var colaborador = await _colaboradorDomainService.BuscarColaboradorPorId(id);
            var usuarioEvent = new UsuarioDeletadoEvent() { Id = colaborador.UsuarioId };
            var usuarioEventRetorno = await _busEvent.RequestAsync<UsuarioDeletadoEvent, UsuarioDeletadoEvent>(usuarioEvent, TimeSpan.FromSeconds(15));

            if (!usuarioEventRetorno.IsDeletado)
                return usuarioEventRetorno.IsDeletado;

            var retorno = await _colaboradorDomainService.Desativar(id);
            var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuarioId ?? string.Empty);
            await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuarioId ?? string.Empty);
            return retorno;
        }
    }
}
