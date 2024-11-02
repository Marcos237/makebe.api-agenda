using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface ILojaPortifolioRepository
    {
        Task<PaginacaoDTO<LojaPortifolioDTO>> BuscarLojaPortifolios(PaginacaoDTO<LojaPortifolioDTO> paginacao, string usuarioId);
        Task<LojaPortifolioDTO> BuscarPorId(int id);
        Task<int> Salvar(LojaPortifolio portifolio);
        Task<LojaPortifolio> Atualizar(LojaPortifolio portifolio);
        Task<bool> Deastivar(int id);
    }
}
