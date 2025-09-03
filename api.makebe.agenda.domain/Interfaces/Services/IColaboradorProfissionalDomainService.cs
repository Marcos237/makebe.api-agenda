using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IColaboradorProfissionalDomainService
    {
        Task<PaginacaoDTO<ColaboradorProfissionalDTO>> BuscarPaginado(PaginacaoDTO<ColaboradorProfissionalDTO> paginacao, string contaId, IEnumerable<UsuarioDTO> usuarios);
        Task<ColaboradorProfissionalDTO> BuscarPorId(int id);
        Task<int> Salvar(ColaboradorProfissional colaborador);
        Task<bool> Desativar(int id);
        Task<IEnumerable<string>> MontarIdsPesquisas(IEnumerable<ColaboradorProfissionalDTO> colaboradores);
        Task<PaginacaoDTO<ColaboradorProfissionalDTO>> MontarColaboradorProfissional(PaginacaoDTO<ColaboradorProfissionalDTO> paginacao, IEnumerable<UsuarioDTO> usuarios);
        Task<PaginacaoDTO<ColaboradorProfissionalDTO>> Filtrar(PaginacaoDTO<ColaboradorProfissionalDTO> paginacao);
        Task<IEnumerable<ColaboradorProfissionalDTO>> BuscarPorConta(string contaId, IEnumerable<UsuarioDTO> usuarios);
    }
}
