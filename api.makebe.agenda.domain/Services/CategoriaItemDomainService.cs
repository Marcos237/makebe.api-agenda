using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class CategoriaItemDomainService : ICategoriaItemDomainService
    {
        private readonly ICategoriaItemRepository _categoriaItemRepository;

        public CategoriaItemDomainService(ICategoriaItemRepository categoriaItemRepository)
        {
            _categoriaItemRepository = categoriaItemRepository;
        }

        public async Task<IEnumerable<CategoriaItem>> BuscarTodosAtivos()
        {
            return await _categoriaItemRepository.BuscarTodosAtivos();
        }

        public async Task<CategoriaItem?> BuscarPorId(int id)
        {
            return await _categoriaItemRepository.BuscarPorId(id);
        }
    }
}
