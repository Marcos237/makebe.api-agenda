using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using api.makebe.agenda.infra.data.interfaces;
using AutoMapper;
using ContasEvent;

namespace api.makebe.agenda.applications.Services.Colaboradores
{
    public class ColaboradorProfissionalApplicationService : IColaboradorProfissionalApplicationService
    {
        private readonly IColaboradorProfissionalDomainService _colaboradorProfissionalDomainService;
        private readonly IMapper _mapper;
        private readonly IValidationService<ColaboradorProfissional> _validationService;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly INotificationContext _notificationContext;
        private readonly IUnitOfWork _unitOfWork;

        public ColaboradorProfissionalApplicationService(IColaboradorProfissionalDomainService ColaboradorProfissionalDomainService, IMapper mapper,
            IValidationService<ColaboradorProfissional> validationService, IBusEvent busEvent, INotificationContext notificationContext,
            IContaEventCrossCuttingService contaEventCrossCuttingService, IUnitOfWork unitOfWork)
        {
            _colaboradorProfissionalDomainService = ColaboradorProfissionalDomainService;
            _mapper = mapper;
            _validationService = validationService;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _notificationContext = notificationContext;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseModel<PaginacaoDTO<ColaboradorProfissionalDTO>>> BuscarUsuariosPaginado(PaginacaoDTO<ColaboradorProfissionalDTO> paginacao, string usuario)
        {
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuario));
            var paginacaoRetorno = await _colaboradorProfissionalDomainService.BuscarPaginado(paginacao, conta?.Id.ToString() ?? string.Empty);
            if (!paginacao.objetos!.Any())
                _validationService.RetornarListaVazia(nameof(ColaboradorProfissional), BaseConstant.ListaVazia);

            return ResponseModelHelper<PaginacaoDTO<ColaboradorProfissionalDTO>>.RetornarResponseModel(paginacaoRetorno, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<ColaboradorProfissionalDTO>> BuscarUsuarioPorId(int id)
        {
            var colaborador = await _colaboradorProfissionalDomainService.BuscarPorId(id);
            if (colaborador.Id == 0)
                _validationService.RetornarListaVazia(nameof(ColaboradorProfissional), BaseConstant.ListaVazia);

            colaborador.PeriodoInativoInicioExtenso = colaborador.PeriodoInativoInicio.ToString("dd/MM/yyyy HH:mm:ss"); 
            colaborador.PeriodoInativoFimExtenso = colaborador.PeriodoInativoFim.ToString("dd/MM/yyyy HH:mm:ss"); ;
            return ResponseModelHelper<ColaboradorProfissionalDTO>.RetornarResponseModel(colaborador, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<ColaboradorProfissionalDTO>> Persistir(ColaboradorProfissionalPayload usuarioPayload)
        {
            if ((usuarioPayload.Servicos == null || !usuarioPayload.Servicos.Any()) && usuarioPayload.ServicoId > 0)
            {
                usuarioPayload.Servicos = new List<ColaboradorServicos>
                {
                    new ColaboradorServicos
                    {
                        IdColaborador = usuarioPayload.ColaboradorId,
                        IdServico = usuarioPayload.ServicoId
                    }
                };
            }

            var colaboradorMap = _mapper.Map<ColaboradorProfissional>(usuarioPayload);
            colaboradorMap.ServicoId = colaboradorMap.Servicos?.FirstOrDefault()?.IdServico ?? usuarioPayload.ServicoId;
            var isValidate = await _validationService.Validar(colaboradorMap);
            if (!isValidate)
            {
                var colaboradorResponseErro = _mapper.Map<ColaboradorProfissionalDTO>(colaboradorMap);
                return ResponseModelHelper<ColaboradorProfissionalDTO>.RetornarResponseModel(colaboradorResponseErro, _notificationContext.Notifications);
            }

            try
            {
                await _unitOfWork.BeginTransaction();
                var retornoColaboradorId = await _colaboradorProfissionalDomainService.Salvar(colaboradorMap);
                _unitOfWork.Commit();
                return await BuscarUsuarioPorId(retornoColaboradorId);
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
        public async Task<bool> Desativar(int id)
        {
            return await _colaboradorProfissionalDomainService.Desativar(id);
        }

        public async Task<ResponseModel<ColaboradorProfissionalDTO>> BuscarPorConta(string usuario)
        {
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuario));
            var colaboradoresResponse = await _colaboradorProfissionalDomainService.BuscarPorConta(conta?.Id.ToString() ?? string.Empty);
            if (colaboradoresResponse.Any() == false)
                _validationService.RetornarListaVazia(nameof(ColaboradorProfissionalDTO), BaseConstant.ListaVazia);

            return ResponseModelHelper<ColaboradorProfissionalDTO>.RetornarResponseModel(colaboradoresResponse, _notificationContext.Notifications);
        }
    }
}
