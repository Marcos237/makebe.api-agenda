using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IServicosDomainService
    {
        Task<IEnumerable<Servicos>> BuscarServicos(string contaId);
        Task<PaginacaoDTO<ServicoDTO>> BuscarTodosPaginado(PaginacaoDTO<ServicoDTO> paginacao, string usuarioId);
        Task<IEnumerable<Servicos>> BuscarServicosPorColaboradoId(int id);
        Task<Servicos> BuscarPorId(int id);
        Task<int> Persitir(Servicos item);
        Task<bool> Desativar(int id);
    }
}
