using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IColaboradorProfissionalRepository
    {
        Task<PaginacaoDTO<ColaboradorProfissionalDTO>> BuscarPaginadoPorContaId(string contaId, PaginacaoDTO<ColaboradorProfissionalDTO> paginacao);
        Task<PaginacaoDTO<ColaboradorProfissionalDTO>> BuscarPaginadoPorUsuario(string usuarioId, PaginacaoDTO<ColaboradorProfissionalDTO> paginacao);
        Task<ColaboradorProfissionalDTO> BuscarPorId(int id);
        Task<IEnumerable<ColaboradorProfissionalDTO>> BuscarPorLojaId(int id);
        Task<int> Salvar(ColaboradorProfissional colaborador);
        Task<bool> Atualizar(ColaboradorProfissional colaborador);
        Task<bool> Desativar(int id);
        Task<IEnumerable<ColaboradorProfissionalDTO>> BuscarPorContaId(string contaId);
        Task<IEnumerable<ColaboradorProfissionalDTO>> BuscarPorUsuarioId(string usuarioId);
    }
}
