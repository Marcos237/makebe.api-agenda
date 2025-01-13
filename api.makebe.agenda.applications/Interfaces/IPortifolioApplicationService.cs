using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IPortifolioApplicationService
    {
        Task<ResponseModel<PaginacaoDTO<PortifolioDTO>>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string usuarioId);
        Task<ResponseModel<PortifolioDTO>> BuscarPorId(int id, int tipoUsuarioPortifolioId = 0);
        Task<ResponseModel<PortifolioDTO>> Persistir(PortifolioPayload portifolio, string UsuarioId);
        Task<bool> Desativar(int id, string usuarioId);
    }
}
