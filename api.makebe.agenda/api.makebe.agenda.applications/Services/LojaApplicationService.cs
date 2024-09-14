using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using AutoMapper;

namespace api.makebe.agenda.applications.Services
{
    public class LojaApplicationService : ILojaApplicationService
    {
        private readonly IValidationService<Loja> _validationService;
        private readonly IUsuarioLojaDomainService _usarioLojaDomainService;
        private readonly IDomainService<Loja> _domainService;
        private readonly INotificationContext _notificationContext;
        private readonly IMapper _mapper;


        public LojaApplicationService(IValidationService<Loja> validationService, IUsuarioLojaDomainService usarioLojaDomainService, IDomainService<Loja> domainService,
            INotificationContext notificationContext, IMapper mapper)
        {
            _domainService = domainService;
            _validationService = validationService;
            _usarioLojaDomainService = usarioLojaDomainService;
            _notificationContext = notificationContext;
            _mapper = mapper;
        }
        public async Task<ResponseModel<LojaResponse>> BuscarTodos(PaginacaoDTO<LojaPayload> lojaPayload, string usuarioId)
        {
            var loja = _mapper.Map<Loja>(lojaPayload.objetoPesquisa) ?? new Loja();
            var paginacaoDTO = new PaginacaoDTO<Loja>() { objetoPesquisa = loja };

            var result = await _domainService.BuscarTodos(paginacaoDTO, usuarioId);
            if (!result.Any())
                _validationService.RetornarListaVazia(BaseConstant.ListaVazia, nameof(Loja));

            var lojaRetorno = _mapper.Map<LojaResponse>(result);
            return ResponseModelHelper<LojaResponse>.RetornarResponseModel(lojaRetorno, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<LojaResponse>> BuscarPorId(int id)
        {
            var result = await _domainService.BuscarPorId(id);
            if (result.Id == 0)
                _validationService.RetornarListaVazia(BaseConstant.ListaVazia, nameof(Loja));

            var lojaRetorno = _mapper.Map<LojaResponse>(result);
            return ResponseModelHelper<LojaResponse>.RetornarResponseModel(lojaRetorno, _notificationContext.Notifications);
        }

        public Task<int> Salvar(LojaPayload item)
        {
            throw new NotImplementedException();
        }


        public Task<LojaResponse> Atualizar(LojaPayload item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Desativar(int id)
        {
            throw new NotImplementedException();
        }
    }
}
