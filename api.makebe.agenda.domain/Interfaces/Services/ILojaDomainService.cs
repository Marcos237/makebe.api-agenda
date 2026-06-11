using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface ILojaDomainService
    {
        Task<IEnumerable<LojaDTO>> BuscarTodos(string usuarioId);
        Task<IEnumerable<LojaVitrineDTO>> BuscarLojasVitrinePorTipo(string tipo);
        Task<PaginacaoDTO<LojaDTO>> BuscarTodosPaginado(PaginacaoDTO<LojaDTO> paginacao, string usuarioId);
        Task<LojaDTO> BuscarPorId(int id); 
        Task<int> Persitir(Loja item);
        Task<bool> Desativar(int id);

    }
}
