using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface ILojaDomainService
    {
        Task<IEnumerable<LojaEnderecoDTO>> BuscarTodos(string usuarioId);
        Task<PaginacaoDTO<LojaEnderecoDTO>> BuscarTodosPaginado(PaginacaoDTO<LojaEnderecoDTO> paginacao, string usuarioId);
        Task<LojaEnderecoDTO> BuscarPorId(int id); 
        Task<int> Persitir(Loja item);
        Task<bool> Desativar(int id);

    }
}
