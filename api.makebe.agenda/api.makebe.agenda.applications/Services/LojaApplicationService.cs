using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.data.interfaces;
using AutoMapper;

namespace api.makebe.agenda.applications.Services
{
    public class LojaApplicationService : AplicationService, ILojaApplicationService
    {
        private readonly IValidationService<Loja> _validationService;
        private readonly IUsuarioLojaDomainService _usarioLojaDomainService;
        private readonly IEnderecoApplicationService _enderecoApplicationService;
        private readonly ILojaDomainService _lojaDomainService;
        private readonly INotificationContext _notificationContext;
        private readonly IMapper _mapper;


        public LojaApplicationService(IValidationService<Loja> validationService, IUsuarioLojaDomainService usarioLojaDomainService, ILojaDomainService lojaDomainService,
            INotificationContext notificationContext, IMapper mapper, IEnderecoApplicationService enderecoApplicationService, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _lojaDomainService = lojaDomainService;
            _validationService = validationService;
            _usarioLojaDomainService = usarioLojaDomainService;
            _notificationContext = notificationContext;
            _enderecoApplicationService = enderecoApplicationService;
            _mapper = mapper;
        }
        public async Task<ResponseModel<LojaResponse>> BuscarTodos(PaginacaoDTO<LojaPayload> lojaPayload, string usuarioId)
        {
            var loja = _mapper.Map<LojaEnderecoDTO>(lojaPayload.objetoPesquisa) ?? new LojaEnderecoDTO();
            var paginacaoDTO = new PaginacaoDTO<LojaEnderecoDTO>() { objetoPesquisa = loja };

            var result = await _lojaDomainService.BuscarTodos(paginacaoDTO, usuarioId);
            if (!result.Any())
                _validationService.RetornarListaVazia(nameof(Loja), BaseConstant.ListaVazia);

            var lojaRetorno = _mapper.Map<IEnumerable<LojaResponse>>(result);
            return ResponseModelHelper<LojaResponse>.RetornarResponseModel(lojaRetorno, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<LojaResponse>> BuscarPorId(int id)
        {
            var result = await _lojaDomainService.BuscarPorId(id);
            if (result.Id == 0)
                _validationService.RetornarListaVazia(BaseConstant.ListaVazia, nameof(Loja));

            var lojaRetorno = _mapper.Map<LojaResponse>(result);
            lojaRetorno.Enderecos = await _enderecoApplicationService.BuscarPorLojaId(id);
            return ResponseModelHelper<LojaResponse>.RetornarResponseModel(lojaRetorno, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<LojaResponse>> Persitir(LojaPayload lojaPayload, string usuarioId)
        {
            var loja = _mapper.Map<Loja>(lojaPayload);
            var isvalidate = await _validationService.Validar(loja);
            bool isValidadeEndereco = await _enderecoApplicationService.ValidarEnderecos(lojaPayload?.Enderecos ?? Enumerable.Empty<Endereco>());
            foreach (var endereco in lojaPayload?.Enderecos!)

                if (isvalidate && isValidadeEndereco)
                    return ResponseModelHelper<LojaResponse>.RetornarResponseModel(new LojaResponse(), _notificationContext.Notifications);
            try
            {
                BeginTransaction();
                var lojaRetorno = await _lojaDomainService.Persitir(loja);
                Guid idGuid = Guid.TryParse(usuarioId, out Guid parsedGuid) ? parsedGuid : Guid.Empty;
                var usuarioLoja = new UsuarioLoja() { LojaId = lojaRetorno, UsuarioId = idGuid };
                await _usarioLojaDomainService.Salvar(usuarioLoja);
                await _enderecoApplicationService.SalvarEnderecos(lojaPayload.Enderecos);
                var lojaResponse = await BuscarPorId(lojaRetorno);
                return lojaResponse;
            }
            catch (Exception)
            {
                RollBack();
                throw;
            }
        }

        public Task<ResponseModel<LojaResponse>> Atualizar(LojaPayload item, string usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Desativar(int id, string usuarioId)
        {
            throw new NotImplementedException();
        }
    }
}
