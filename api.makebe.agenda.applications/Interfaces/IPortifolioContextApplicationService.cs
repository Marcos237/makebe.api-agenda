using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IPortifolioContextApplicationService
    {
        Task<PaginacaoDTO<PortifolioDTO>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string usuarioId);

        Task<int> Salvar(PortifolioPayload portifolio);
    }
}
