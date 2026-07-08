using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class CategoriaDomainService : ICategoriaDomainService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaDomainService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<int> Salvar(Categoria categoria)
        {
            return await _categoriaRepository.Salvar(categoria);
        }

        public async Task<IEnumerable<Categoria>> BuscarPorServico(int servicoId)
        {
            return await _categoriaRepository.BuscarPorServico(servicoId);
        }

        public async Task<bool> DesativarPorServico(int servicoId)
        {
            return await _categoriaRepository.DesativarPorServico(servicoId);
        }
    }
}
