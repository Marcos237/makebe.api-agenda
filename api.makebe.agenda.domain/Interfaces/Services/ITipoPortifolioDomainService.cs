using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface ITipoPortifolioDomainService
    {
        Task<IEnumerable<TipoPortifolioDTO>> BuscarPorTipoUsuarioPortifolioId(int tipoPortifolioId);
    }
}
