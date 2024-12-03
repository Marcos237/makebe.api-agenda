using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebesession.infra.crosscutting.Events.Usuarios;
using AutoMapper;

namespace api.makebe.agenda.applications.Services
{
    public class ColaboradorApplicationService : IColaboradorApplicationService
    {
        private readonly IBusEvent _busEvent;
        private readonly INotificationContext _notificationContext;
        private readonly IMapper _mapper;
        public ColaboradorApplicationService(IBusEvent busEvent, INotificationContext notificationContext, IMapper mapper)
        {
            _busEvent = busEvent;
            _notificationContext = notificationContext;
            _mapper = mapper;
    }
        public async Task<ResponseModel<ColaboradorDTO>> BuscarUsuario(ColaboradorPayload usuarioPayload, string usuario)
        {
            var usuarioMap = _mapper.Map<UsuarioConsultadoEvent>(usuarioPayload) ?? new UsuarioConsultadoEvent();
            var usuarioEvent = await _busEvent.RequestAsync<UsuarioConsultadoEvent, UsuarioConsultadoEvent>(usuarioMap, TimeSpan.FromSeconds(15));
            //return ResponseModelHelper<PaginacaoEvent<UsuarioEvent>>.RetornarResponseModel(new PaginacaoEvent<UsuarioEvent>(), _notificationContext.Notifications);
            return new ResponseModel<ColaboradorDTO>();
        }

        public async Task<ResponseModel<ColaboradorDTO>> SalvarUsuario(ColaboradorPayload usuarioPayload, string usuario)
        {
            var usuarioMap = _mapper.Map<UsuarioRegistradoEvent>(usuarioPayload);
            var usuarioEvent = await _busEvent.RequestAsync<UsuarioRegistradoEvent, UsuarioRegistradoEvent>(usuarioMap, TimeSpan.FromSeconds(15));
            return new ResponseModel<ColaboradorDTO>();
        }
    }
}
