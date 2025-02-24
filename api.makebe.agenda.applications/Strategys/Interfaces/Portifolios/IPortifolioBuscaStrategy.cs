using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Strategys.Interfaces.Portifolios
{
    public interface IPortifolioBuscaStrategy
    {
        Task<PaginacaoDTO<PortifolioDTO>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string usuarioId);
    }
}
