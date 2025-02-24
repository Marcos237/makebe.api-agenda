using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IEnderecoApplicationService
    {
        Task<ResponseModel<EnderecoDTO>> Persistir(EnderecoPayload enderecos, string usuarioId);
        Task<ResponseModel<EnderecoDTO>> BuscarPorId(int lojaId);
        Task<ResponseModel<PaginacaoDTO<EnderecoDTO>>> BuscarTodos(PaginacaoDTO<EnderecoDTO> paginacao, string usuarioId);
        Task<bool> DesativarEnderecos(int id);
    }
}
