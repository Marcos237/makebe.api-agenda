using api.makebe.agenda.applications.Strategys.Interfaces;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Enums;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;

namespace api.makebe.agenda.applications.Strategys.Services.Portifolios
{
    public class LojaPortifolioBuscaStrategy : IPortifolioBuscaStrategy 
    {
        private readonly ILojaPortifolioDomainService _lojaPortifolioDomainService;
        private readonly IContaEventCrossCuttingService _contaEventCrossCuttingService;

        public LojaPortifolioBuscaStrategy(ILojaPortifolioDomainService lojaPortifolioDomainService, IContaEventCrossCuttingService contaEventCrossCuttingService)
        {
            _lojaPortifolioDomainService = lojaPortifolioDomainService;
            _contaEventCrossCuttingService = contaEventCrossCuttingService; 
        }
        public async Task<PaginacaoDTO<PortifolioDTO>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string usuarioId)
        {
            if (paginacao.objetoPesquisa?.TipoUsuarioPortifolioId == (int)TipoUsuarioPortifolio.Loja)
            {
                var conta = await _contaEventCrossCuttingService.BuscarContaPorId(PropiedadesHelper.ParseGuidOrDefault(usuarioId));
                var response = await _lojaPortifolioDomainService.BuscarPortifolios(paginacao, conta.Id.ToString() ?? string.Empty);
                return response;
            }
            return paginacao;
        }
    }
}
