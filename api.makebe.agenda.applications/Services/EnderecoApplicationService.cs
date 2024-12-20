using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.data.interfaces;
using api.makebesession.infra.crosscutting.Events.Contas;
using AutoMapper;
using lib.makebe.domain.Interfaces.Services;

namespace api.makebe.agenda.applications.Services
{
    public class EnderecoApplicationService : IEnderecoApplicationService
    {
        private readonly IEnderecoDomainService _enderecoDomainService;
        private readonly ILojaEnderecoApplicationService _lojaEnderecoApplicationService;
        private readonly IValidationService<Endereco> _validationService;
        private readonly INotificationContext _notificationContext;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioSessaoDomainService _usuarioSessaoDomainService;
        private readonly IBusEvent _busEvent;
        public EnderecoApplicationService(IEnderecoDomainService enderecoDomainService, IValidationService<Endereco> validationService, IMapper mapper,
            INotificationContext notificationContext, ILojaEnderecoApplicationService lojaEnderecoApplicationService, IUnitOfWork unitOfWork, IBusEvent busEvent,
            IUsuarioSessaoDomainService usuarioSessaoDomainService)
        {
            _enderecoDomainService = enderecoDomainService;
            _validationService = validationService;
            _mapper = mapper;
            _notificationContext = notificationContext;
            _lojaEnderecoApplicationService = lojaEnderecoApplicationService;
            _unitOfWork = unitOfWork;
            _usuarioSessaoDomainService = usuarioSessaoDomainService;
            _busEvent = busEvent;
        }
        public async Task<ResponseModel<PaginacaoDTO<EnderecoDTO>>> BuscarTodos(PaginacaoDTO<EnderecoDTO> paginacao, string usuarioId)
        {
            var conta = await ContaHelper.BuscarContaPorUsuarioId(usuarioId, _busEvent);
            var paginacaoRetorno = await _enderecoDomainService.BuscarTodos(paginacao, conta.Id.ToString() ?? string.Empty) ?? new PaginacaoDTO<EnderecoDTO>();
            if (paginacaoRetorno != null && !paginacaoRetorno.objetos!.Any())
                _validationService.RetornarListaVazia(nameof(Endereco), BaseConstant.ListaVazia);

            return ResponseModelHelper<PaginacaoDTO<EnderecoDTO>>.RetornarResponseModel(paginacaoRetorno!, _notificationContext.Notifications);

        }
        public async Task<ResponseModel<EnderecoDTO>> BuscarPorId(int lojaId)
        {
            var retorno = await _enderecoDomainService.BuscarPorId(lojaId);
            if (retorno.Id == 0)
                _validationService.RetornarListaVazia(nameof(Endereco), BaseConstant.ListaVazia);

            return ResponseModelHelper<EnderecoDTO>.RetornarResponseModel(retorno!, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<EnderecoDTO>> Persistir(EnderecoDTO enderecoDTO, string usuarioId)
        {
            var endereco = _mapper.Map<Endereco>(enderecoDTO);
            var isValidate = await _validationService.Validar(endereco);
            if (!isValidate)
            {
                var lojaErro = _mapper.Map<EnderecoDTO>(endereco);
                return ResponseModelHelper<EnderecoDTO>.RetornarResponseModel(lojaErro, _notificationContext.Notifications);
            }
            try
            {
                await _unitOfWork.BeginTransaction();
                var enderecoRetorno = await _enderecoDomainService.Salvar(endereco);
                var lojaEndereco = new LojaEndereco { EnderecoId = enderecoRetorno, LojaId = enderecoDTO.LojaId };
                await _lojaEnderecoApplicationService.SalvarLojaEndereco(lojaEndereco);
                _unitOfWork.Commit();
                var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuarioId ?? string.Empty);
                await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuarioId ?? string.Empty);

                var retornoEndereco = await BuscarPorId(enderecoRetorno);
                return retornoEndereco;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }

        }

        public async Task<bool> DesativarEnderecos(int id)
        {
            var endercoRetorno = await _enderecoDomainService.Desativar(id);
            return endercoRetorno;
        }
    }
}
