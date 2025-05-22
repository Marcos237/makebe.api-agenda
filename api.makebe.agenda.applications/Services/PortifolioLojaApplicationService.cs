using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using AutoMapper;

namespace api.makebe.agenda.applications.Services
{
    public class PortifolioLojaApplicationService : IPortifolioContextApplicationService
    {
        private readonly ILojaPortifolioDomainService _lojaPortifolioDomainService;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;
        private readonly IMapper _mapper;

        public PortifolioLojaApplicationService(ILojaPortifolioDomainService lojaPortifolioDomainService, IContaEventCrossCuttingService contaEventCrossCuttingService, IMapper mapper)
        {
            _lojaPortifolioDomainService = lojaPortifolioDomainService;
            _contaEventCrossCuttingService = contaEventCrossCuttingService;
            _mapper = mapper;   
        }
        public async Task<PaginacaoDTO<PortifolioDTO>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string usuarioId)
        {
            var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
            var response = await _lojaPortifolioDomainService.BuscarPortifolios(paginacao, conta.Id.ToString() ?? string.Empty);
            return response;
        }

        public async Task<int> Salvar(PortifolioPayload portifolio)
        {
            var lojaMap = _mapper.Map<LojaPortifolio>(portifolio);
            var response = await _lojaPortifolioDomainService.Salvar(lojaMap);
            return response == 0 ? 0 : response;
        }
    }
}
