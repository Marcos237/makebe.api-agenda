using api.makebe.agenda.applications.Factorys.Interfaces;
using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Enums;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.data.interfaces;
using AutoMapper;
using lib.makebe.domain.Interfaces.Services;

namespace api.makebe.agenda.applications.Services.Enderecos
{
    public class EnderecoApplicationService : IEnderecoApplicationService
    {
        private readonly IEnderecoDomainService _enderecoDomainService;
        private readonly IEnderecoValidacaoApplicationService _enderecoValidacaoApplicationService;
        private readonly INotificationContext _notificationContext;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioSessaoDomainService _usuarioSessaoDomainService;
        private readonly IContextFactory<IEnderecoContextApplicationService> _contextFactory;

        public EnderecoApplicationService(IEnderecoDomainService enderecoDomainService, IEnderecoValidacaoApplicationService enderecoValidacaoApplicationService, IMapper mapper,
            INotificationContext notificationContext, IUnitOfWork unitOfWork,
            IUsuarioSessaoDomainService usuarioSessaoDomainService, IContextFactory<IEnderecoContextApplicationService> contextFactory)
        {
            _enderecoDomainService = enderecoDomainService;
            _enderecoValidacaoApplicationService = enderecoValidacaoApplicationService;
            _mapper = mapper;
            _notificationContext = notificationContext;
            _unitOfWork = unitOfWork;
            _usuarioSessaoDomainService = usuarioSessaoDomainService;
            _contextFactory = contextFactory;
        }
        public async Task<ResponseModel<PaginacaoDTO<EnderecoDTO>>> BuscarTodos(PaginacaoDTO<EnderecoDTO> paginacao, string usuarioId)
        {
            var instancia = await _contextFactory.ExecutarService(paginacao?.objetoPesquisa?.TipoUsuarioId ?? 0);
            var paginacaoRetorno = await instancia.BuscarEnderecos(paginacao!, usuarioId);
            if (paginacaoRetorno != null && !paginacaoRetorno.objetos!.Any())
                _enderecoValidacaoApplicationService.RetornarListaVazia(nameof(Endereco), BaseConstant.ListaVazia);

            return ResponseModelHelper<PaginacaoDTO<EnderecoDTO>>.RetornarResponseModel(paginacaoRetorno!, _notificationContext.Notifications);

        }
        public async Task<ResponseModel<EnderecoDTO>> BuscarPorId(int lojaId)
        {
            var retorno = await _enderecoDomainService.BuscarPorId(lojaId);
            if (retorno.Id == 0)
                _enderecoValidacaoApplicationService.RetornarListaVazia(nameof(Endereco), BaseConstant.ListaVazia);

            return ResponseModelHelper<EnderecoDTO>.RetornarResponseModel(retorno!, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<EnderecoDTO>> Persistir(EnderecoPayload enderecoPayload, string usuarioId)
        {
            var endereco = _mapper.Map<Endereco>(enderecoPayload);
            var isValidate = await _enderecoValidacaoApplicationService.Validar(enderecoPayload);
            if (!isValidate)
            {
                var lojaErro = _mapper.Map<EnderecoDTO>(endereco);
                return ResponseModelHelper<EnderecoDTO>.RetornarResponseModel(lojaErro, _notificationContext.Notifications);
            }
            try
            {
                await _unitOfWork.BeginTransaction();
                var enderecoRetorno = await _enderecoDomainService.Salvar(endereco);
                enderecoPayload.Id = enderecoRetorno;
                var instancia = await _contextFactory.ExecutarService(enderecoPayload.TipoUsuarioId);
                await instancia.Salvar(enderecoPayload);
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
