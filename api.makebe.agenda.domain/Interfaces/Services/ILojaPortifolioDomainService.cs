using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface ILojaPortifolioDomainService
    {
        Task<PaginacaoDTO<PortifolioDTO>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string contaId);
        Task<int> Salvar(LojaPortifolio item);
        Task<PaginacaoDTO<PortifolioDTO>> Filtrar(PaginacaoDTO<PortifolioDTO> paginacao);
    }
}
