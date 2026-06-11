using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using PesquisarVitrineEvent;

namespace api.makebe.agenda.applications.Services.Vitrines
{
    public class VitrineService : IVitrineService
    {
        private readonly IVitrineRepository _vitrineRepository;

        public VitrineService(IVitrineRepository vitrineRepository)
        {
            _vitrineRepository = vitrineRepository;
        }

        public async Task<List<ItemVitrineResponse>> PesquisarAsync(string valorItem, CancellationToken cancellationToken)
        {
            return await _vitrineRepository.PesquisarAsync(valorItem, cancellationToken);
        }
    }
}
