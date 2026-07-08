using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface ICategoriaItemRepository
    {
        Task<IEnumerable<CategoriaItem>> BuscarTodosAtivos();
        Task<CategoriaItem?> BuscarPorId(int id);
    }
}
