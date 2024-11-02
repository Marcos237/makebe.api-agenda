using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface ILojaPortifolioApplicationService
    {
        Task<ResponseModel<PaginacaoDTO<LojaPortifolioDTO>>> BuscarLojaPortifolios(PaginacaoDTO<LojaPortifolioDTO> paginacao, string usuarioId);
        Task<ResponseModel<LojaPortifolioDTO>> BuscarPorId(int id);
        Task<ResponseModel<LojaPortifolioDTO>> Persistir(LojaPortifolioPayload portifolio, string UsuarioId);
        Task<bool> Desativar(int id, string usuarioId);
    }
}
