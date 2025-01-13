using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class TipoPortifolioDomainService : ITipoPortifolioDomainService
    {
        private readonly ITipoPortifolioRepository _tipoPortifolioRepository;
        public TipoPortifolioDomainService(ITipoPortifolioRepository tipoPortifolioRepository)
        {
            _tipoPortifolioRepository = tipoPortifolioRepository;
        }
        public async Task<IEnumerable<TipoPortifolioDTO>> BuscarPorTipoUsuarioPortifolioId(int tipoPortifolioId)
        {
            return await _tipoPortifolioRepository.BuscarPorTipoUsuarioPortifolioId(tipoPortifolioId);
        }
    }
}
