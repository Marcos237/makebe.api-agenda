using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IColaboradorPortifolioRepository : IPortifolioContextRepository<ColaboradorPortifolio, PortifolioDTO>
    {
        Task<PaginacaoDTO<PortifolioDTO>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string contaId);
    }
}
