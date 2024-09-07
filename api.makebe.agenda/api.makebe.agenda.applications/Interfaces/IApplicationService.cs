using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IApplicationService<T> where T : class
    {
        Task<ResponseModel<T>> BuscarTodos(PaginacaoDTO<T> paginacaoDTO, string usuarioId);
        Task<ResponseModel<T>> BuscarPorId(int id);
        Task<int> Salvar(T item);
        Task<T> Atualizar(T item);
        Task<bool> Desativar(int id);
    }
}
