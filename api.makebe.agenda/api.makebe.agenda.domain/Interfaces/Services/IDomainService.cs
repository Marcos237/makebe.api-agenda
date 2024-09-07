using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IDomainService<T> where T : class
    {
        Task<IEnumerable<T>> BuscarTodos(PaginacaoDTO<T> paginacao, string usuarioId);
        Task<T> BuscarPorId(int id); 
        Task<int> Salvar(T item);
        Task<T> Atualizar(T item);
        Task<bool> Desativar(int id);

    }
}
