using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IColaboradorRepository
    {
        Task<ColaboradorDTO> BuscarPorUsuarioId(Guid id);
        Task<ColaboradorDTO> BuscarPorId(int id);
        Task<int> Salvar(Colaborador colaborador);
        Task<Colaborador> Atualizar(Colaborador colaborador);
        Task<bool> Desativar(int id);
        Task<PaginacaoDTO<ColaboradorDTO>> BuscarPaginadoPorConta(string usuarioId, PaginacaoDTO<ColaboradorDTO> paginacao);
        Task<IEnumerable<ColaboradorDTO>> BuscarPorConta(string usuarioId);
    }
}
