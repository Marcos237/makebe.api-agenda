using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface ITipoPortifolioApplicationService
    {
        Task<ResponseModel<TipoPortifolioDTO>> BuscarPorTipoUsuarioId(int tipoPortifolioId);
    }
}
