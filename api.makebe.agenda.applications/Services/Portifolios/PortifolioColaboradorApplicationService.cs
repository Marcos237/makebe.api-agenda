using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using AutoMapper;
using ContasEvent;

namespace api.makebe.agenda.applications.Services.Portifolios
{
    public class PortifolioColaboradorApplicationService : IPortifolioContextApplicationService
    {
        private readonly IColaboradorPortifolioDomainService _colaboradorPortifolioDomainService;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly IMapper _mapper;

        public PortifolioColaboradorApplicationService(IColaboradorPortifolioDomainService colaboradorPortifolioDomainService, IContaEventCrossCuttingService contaEventCrossCuttingService,
            IMapper mapper)
        {
            _colaboradorPortifolioDomainService = colaboradorPortifolioDomainService;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _mapper = mapper;
        }
        public async Task<PaginacaoDTO<PortifolioDTO>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string usuarioId)
        {
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
            var usuarioConsultadoEvent = new UsuarioContaConsultadoPorContaEvent() { IdConta = conta.Id ?? Guid.Empty };
            var usuariosConta = await _contaEventCrossCuttingService.BuscarUsuarioContaPorIdConta(usuarioConsultadoEvent);
            var usuariosMap = _mapper.Map<IEnumerable<UsuarioDTO>>(usuariosConta.UsuariosEvents);

            var response = await _colaboradorPortifolioDomainService.BuscarPortifolios(paginacao, conta.Id.ToString() ?? string.Empty, usuariosMap);
            return response;
        }

        public async Task<int> Salvar(PortifolioPayload portifolio)
        {
            var colaboradorMap = _mapper.Map<ColaboradorPortifolio>(portifolio);
            var response = await _colaboradorPortifolioDomainService.Salvar(colaboradorMap);
            return response == 0 ? 0 : response;
        }
    }
}
