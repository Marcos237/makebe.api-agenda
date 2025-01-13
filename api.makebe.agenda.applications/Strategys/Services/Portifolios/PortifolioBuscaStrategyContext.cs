using api.makebe.agenda.applications.Strategys.Interfaces;
using api.makebe.agenda.domain.DTO;

public class PortifolioBuscaStrategyContext: IPortifolioBuscaStrategyContext
{
    private readonly IEnumerable<IPortifolioBuscaStrategy> _portifolioBuscaStrategies;
    public PortifolioBuscaStrategyContext(IEnumerable<IPortifolioBuscaStrategy> portifolioBuscaStrategies)
    {
        _portifolioBuscaStrategies = portifolioBuscaStrategies;
    }
    public async Task<PaginacaoDTO<PortifolioDTO>> Buscar(PaginacaoDTO<PortifolioDTO> paginacao, string contaId)
    {

        foreach (var strategy in _portifolioBuscaStrategies)
        {
            paginacao = await strategy.BuscarPortifolios(paginacao, contaId);
        }
        return paginacao;
    }
}
