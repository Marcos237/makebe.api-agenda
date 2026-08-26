using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IColaboradorDomainService
    {
        Task<int> Salvar(Colaborador colaborador, string id);
        Task<ColaboradorDTO> BuscarColaboradorPorIdUsuario(Guid id);
        Task<ColaboradorDTO> BuscarColaboradorPorId(int id);
        Task<bool> Desativar(int id);
        Task<PaginacaoDTO<ColaboradorDTO>> BuscarPaginadoPorConta(string usuarioId, PaginacaoDTO<ColaboradorDTO> paginacao);
        Task<IEnumerable<ColaboradorDTO>> BuscarPorConta(string usuarioId);
        Task<ColaboradorDTO> BuscarColaboradorUsuarioPorId(string id);
    }
}
