using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface ILojaDomainService
    {
        Task<IEnumerable<LojaEnderecoDTO>> BuscarTodos(PaginacaoDTO<LojaEnderecoDTO> paginacao, string usuarioId);
        Task<Loja> BuscarPorId(int id); 
        Task<int> Persitir(Loja item);
        Task<bool> Desativar(int id);

    }
}
