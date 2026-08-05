using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IServicosDomainService
    {
        Task<IEnumerable<Servico>> BuscarServicos(string contaId);
        Task<PaginacaoDTO<ServicoDTO>> BuscarTodosPaginado(PaginacaoDTO<ServicoDTO> paginacao, string usuarioId);
        Task<IEnumerable<Servico>> BuscarServicosPorColaboradorId(int id);
        Task<Servico> BuscarPorId(int id);
        Task<int> Persitir(Servico item);
        Task<bool> Desativar(int id);
    }
}
