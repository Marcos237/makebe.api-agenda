using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Strategys.Interfaces
{
    public interface IPortifolioBuscaStrategy
    {
        Task<PaginacaoDTO<PortifolioDTO>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string usuarioId);
    }
}
