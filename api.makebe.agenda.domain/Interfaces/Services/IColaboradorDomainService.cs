using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.infra.crosscutting.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IColaboradorDomainService
    {
        Task<int> Salvar(Colaborador colaborador, string id);
        Task<ColaboradorDTO> BuscarColaboradorPorIdUsuario(Guid id);
        Task<ColaboradorDTO> BuscarColaboradorPorId(int id);
        Task<bool> Desativar(int id);
        Task<IEnumerable<string>> MontarIdsPesquisas(string usuarioId);
        Task<PaginacaoDTO<ColaboradorDTO>> MontarColaboradoresPaginado(PaginacaoDTO<UsuarioDTO>? paginacao, string usuarioId, IEnumerable<PermissaoEvent> permissoesEvents);
        Task<IEnumerable<ColaboradorDTO>> MontarColaboradores(IEnumerable<UsuarioDTO>? usuarios, string contaId);
    }
}
