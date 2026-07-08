using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface ICategoriaItemDomainService
    {
        Task<IEnumerable<CategoriaItem>> BuscarTodosAtivos();
        Task<CategoriaItem?> BuscarPorId(int id);
    }
}
