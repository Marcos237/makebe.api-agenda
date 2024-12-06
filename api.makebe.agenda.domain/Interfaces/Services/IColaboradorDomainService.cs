using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IColaboradorDomainService
    {
        Task<int> Salvar(Colaborador colaborador);
        Task<ColaboradorDTO> BuscarColaboradorPorIdUsuario(Guid id);
        Task<ColaboradorDTO> BuscarColaboradorPorId(int id);
        Task<bool> Desativar(int id);
        Task<PaginacaoDTO<ColaboradorDTO>> MontarColaboradores(PaginacaoDTO<UsuarioDTO>? paginacao, string usuarioId);
    }
}
