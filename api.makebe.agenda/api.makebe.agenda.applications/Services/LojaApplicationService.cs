using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;

namespace api.makebe.agenda.applications.Services
{
    public class LojaApplicationService : IApplicationService<Loja>
    {
        private readonly IValidationService<Loja> _validationService;
        private readonly IUsuarioLojaDomainService _usarioLojaDomainService;
        private readonly IDomainService<Loja> _domainService;
        private INotificationContext _notificationContext;

        public LojaApplicationService(IValidationService<Loja> validationService, IUsuarioLojaDomainService usarioLojaDomainService, IDomainService<Loja> domainService,
            INotificationContext notificationContext)
        {
            _domainService = domainService;
            _validationService = validationService;
            _usarioLojaDomainService = usarioLojaDomainService;
            _notificationContext = notificationContext;
        }
        public async Task<ResponseModel<Loja>> BuscarTodos(PaginacaoDTO<Loja> paginacaoDTO, string usuarioId)
        {
            var result = await _domainService.BuscarTodos(paginacaoDTO, usuarioId);
            if (!result.Any())
                _validationService.RetornarListaVazia(BaseConstant.ListaVazia, nameof(Loja));

            return ResponseModelHelper<Loja>.RetornarResponseModel(result, _notificationContext.Notifications);
        }

        public Task<ResponseModel<Loja>> BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }



        public Task<Loja> Atualizar(Loja item)
        {
            throw new NotImplementedException();
        }



        public Task<bool> Desativar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Salvar(Loja item)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ResponseModel<Loja>>> BuscarTodos(PaginacaoDTO<Loja> paginacaoDTO)
        {
            throw new NotImplementedException();
        }
    }
}
