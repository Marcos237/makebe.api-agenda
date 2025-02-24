using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface ITipoPortifolioRepository
    {
        Task<IEnumerable<TipoPortifolioDTO>> BuscarPorTipoUsuarioId(int  tipoPortifolioId);
    }
}
