using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IColaboradorPortifolioDomainService
    {
        Task<PaginacaoDTO<PortifolioDTO>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string contaId, IEnumerable<UsuarioDTO> usuarios);
        Task<int> Salvar(ColaboradorPortifolio item);
        Task<PaginacaoDTO<PortifolioDTO>> MontarColaborador(PaginacaoDTO<PortifolioDTO> paginacao, IEnumerable<UsuarioDTO> usuarios);
        Task<PaginacaoDTO<PortifolioDTO>> Filtrar(PaginacaoDTO<PortifolioDTO> paginacao);
    }
}
