using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Extensions;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using AutoMapper;
using ContasEvent;
using lib.makebe.domain.Interfaces.Services;
using UsuariosEvent;

namespace api.makebe.agenda.applications.Services.Agendamentos
{
    public class AgendamentoApplicationService : IAgendamentoApplicationService
    {
        private readonly IAgendamentoDomainService _agendamentoDomainService;
        private readonly INotificationContext _notificationContext;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly IUsuarioEventCrossCuttingService _usuarioEventCrossCuttingService;
        private readonly IUsuarioClienteConsultadosCrosCuttingService _consultadosCrosCuttingService;
        private readonly IMapper _mapper;
        private readonly IValidationService<AgendamentoDTO> _validationService;
        private readonly IUsuarioSessaoDomainService _usuarioSessaoDomainService;
        private readonly IServicosDomainService _servicosDomainService;
        private readonly IEmailEnvioDomainService _emailEnvioDomainService;

        public AgendamentoApplicationService(IAgendamentoDomainService agendamentoDomainService, INotificationContext notificationContext,
            IContaEventCrossCuttingService contaEventCrossCuttingService, IUsuarioEventCrossCuttingService usuarioEventCrossCuttingService,
            IUsuarioClienteConsultadosCrosCuttingService consultadosCrosCuttingService, IMapper mapper, IValidationService<AgendamentoDTO> validationService,
            IUsuarioSessaoDomainService usuarioSessaoDomainService, IServicosDomainService servicosDomainService,
            IEmailEnvioDomainService emailEnvioDomainService)
        {
            _agendamentoDomainService = agendamentoDomainService;
            _notificationContext = notificationContext;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _consultadosCrosCuttingService = consultadosCrosCuttingService;
            _usuarioEventCrossCuttingService = usuarioEventCrossCuttingService;
            _mapper = mapper;
            _validationService = validationService;
            _usuarioSessaoDomainService = usuarioSessaoDomainService;
            _servicosDomainService = servicosDomainService;
            _emailEnvioDomainService = emailEnvioDomainService;
        }
        public async Task<ResponseModel<PaginacaoDTO<AgendamentoDTO>>> BuscarAgendamentoPaginado(PaginacaoDTO<AgendamentoDTO> paginacao, string usuario)
        {
            var clientes = await _consultadosCrosCuttingService.BuscarUsuarioClientes();
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuario));
            var contaEvent = new UsuarioContaConsultadoPorContaEvent() { IdConta = PropiedadesHelper.ParseGuidOrDefault(conta.Id.ToString()) };
            var colaboradores = await _contaEventCrossCuttingService.BuscarUsuarioContaPorIdConta(contaEvent);
            var response = await _agendamentoDomainService.MontarAgendamento(paginacao, conta?.Id?.ToString() ?? string.Empty, clientes, colaboradores.UsuariosEvents);
            var responseFilter = _agendamentoDomainService.Filtrar(response);

            return ResponseModelHelper<PaginacaoDTO<AgendamentoDTO>>.RetornarResponseModel(new PaginacaoDTO<AgendamentoDTO>(), _notificationContext.Notifications);
        }

        public async Task<ResponseModel<AgendamentoDTO>> BuscarAgendamentoPorId(int id)
        {
            if (id == 0)
                return ResponseModelHelper<AgendamentoDTO>.RetornarResponseModel(new AgendamentoDTO(), _notificationContext.Notifications);
            var response = await _agendamentoDomainService.BuscarPorId(id);
            var usuarioEvent = new UsuarioConsultadoPorIdEvent() { Id = PropiedadesHelper.ParseGuidOrDefault(response.IdUsuario) };
            var responseEvent = await _usuarioEventCrossCuttingService.BuscarUsuarioPorId(usuarioEvent);
            response.NomeUsuario = responseEvent.UsuarioConsultadoRetorno?.Nome ?? string.Empty;

            return ResponseModelHelper<AgendamentoDTO>.RetornarResponseModel(response, _notificationContext.Notifications);
        }

        public async Task<PaginacaoDTO<AgendamentoConsultaDTO>> BuscarMeusAgendamentos(PaginacaoDTO<AgendamentoConsultaDTO> paginacao, string usuarioId)
        {
            return await _agendamentoDomainService.BuscarMeusAgendamentos(paginacao, usuarioId);
        }

        public async Task<ResponseModel<AgendamentoDTO>> BuscarAgendamentoPorAno(int ano, int id, string usuarioId)
        {
            var usuario = PropiedadesHelper.ParseGuidOrDefault(usuarioId);
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(usuario);
            var idsUsuarios = new List<string>();
            var response = await _agendamentoDomainService.BuscarPorAnoConta(ano, id, conta?.Id.ToString() ?? string.Empty);
            foreach (var agendamento in response)
                idsUsuarios.Add(agendamento?.IdUsuario ?? string.Empty);

            var eventos = new UsuariosConsutadosPorIdsEvent() { Ids = idsUsuarios };
            var usuariosEvent = (await _usuarioEventCrossCuttingService.BuscarUsuariosPorIds(eventos)).UsuariosConsultadosRetorno ??
                Enumerable.Empty<UsuarioEvent>();

            foreach (var agendamento in response)
                agendamento.NomeUsuario = usuariosEvent.Where(usuario => PropiedadesHelper.GuidToStringOrEmpty(usuario.Id) == agendamento.IdUsuario).FirstOrDefault()?.Nome;

            return ResponseModelHelper<AgendamentoDTO>.RetornarResponseModel(response, _notificationContext.Notifications);
        }

        public async Task<ResponseModel<AgendamentoDTO>> BuscarAgendamentoPorData(string? data, int id, string usuarioId)
        {
            if (string.IsNullOrEmpty(data))
                return ResponseModelHelper<AgendamentoDTO>.RetornarResponseModel(new AgendamentoDTO(), _notificationContext.Notifications);

            var usuario = PropiedadesHelper.ParseGuidOrDefault(usuarioId);
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(usuario);
            var dataFormat = Convert.ToDateTime(data);
            var response = await _agendamentoDomainService.BuscarAgendamentoPorData(dataFormat, id, conta?.Id.ToString() ?? string.Empty);
            return ResponseModelHelper<AgendamentoDTO>.RetornarResponseModel(response, _notificationContext.Notifications);
        }
        public async Task<ResponseModel<AgendamentoDTO>> Persistir(AgendamentoDTO agendamentoDTO, string usuario)
        {
            var servico = await _servicosDomainService.BuscarPorId(agendamentoDTO.IdServico);
            agendamentoDTO.Periodo = servico?.Periodo ?? 0;
            agendamentoDTO.DataInicioAgendamento = ValoresHelper.MontarDate(agendamentoDTO.DataInicioAgendamentoExtenso, agendamentoDTO.Data) ?? DateTime.Now;
            agendamentoDTO.DataTerminoAgendamento = agendamentoDTO.MontarDataTermino();

            Console.WriteLine($"MontarDate={agendamentoDTO.DataInicioAgendamento}");

            var isValid = await _validationService.Validar(agendamentoDTO);
            if (!isValid)
                return ResponseModelHelper<AgendamentoDTO>.RetornarResponseModel(agendamentoDTO, _notificationContext.Notifications);

            var agendamentoMap = _mapper.Map<Agendamento>(agendamentoDTO);
            var idColaborador = Convert.ToInt32(TextoHelper.GetNumeros(agendamentoDTO.IdColaborador ?? "0"));
            var response = await _agendamentoDomainService.Salvar(agendamentoMap, idColaborador);
            agendamentoDTO.Id = response;

            var retornoSessaoAtual = await _usuarioSessaoDomainService.BuscarSessao(usuario ?? string.Empty);
            await _usuarioSessaoDomainService.AtualizarSessao(retornoSessaoAtual, usuario ?? string.Empty);

            if (response > 0)
                await _emailEnvioDomainService.GerarEmailsAgendamento(agendamentoDTO);

            return ResponseModelHelper<AgendamentoDTO>.RetornarResponseModel(agendamentoDTO, _notificationContext.Notifications);
        }

        public async Task<bool> Desativar(int id)
        {
            if (id == 0)
                return false;

            var response = await _agendamentoDomainService.Desativa(id);
            return response;
        }

    }
}
