using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebesession.infra.crosscutting.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IColaboradorDomainService
    {
        Task<int> Salvar(Colaborador colaborador);
        Task<ColaboradorDTO> BuscarColaboradorPorIdUsuario(Guid id);
        Task<ColaboradorDTO> BuscarColaboradorPorId(int id);
        Task<bool> Desativar(int id);
        Task<IEnumerable<string>> MontarIdsPesquisas(string usuarioId);
        Task<PaginacaoDTO<ColaboradorDTO>> MontarColaboradores(PaginacaoDTO<UsuarioDTO>? paginacao, string usuarioId, IEnumerable<PermissaoEvent> permissoesEvents);
    }
}
