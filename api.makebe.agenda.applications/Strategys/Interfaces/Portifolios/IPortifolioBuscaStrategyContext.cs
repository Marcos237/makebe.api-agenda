using api.makebe.agenda.domain.DTO;

public interface IPortifolioBuscaStrategyContext
{
    Task<PaginacaoDTO<PortifolioDTO>> Buscar(PaginacaoDTO<PortifolioDTO> paginacao, string contaId);
}
