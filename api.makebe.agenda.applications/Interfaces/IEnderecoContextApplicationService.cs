using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IEnderecoContextApplicationService
    {
        Task<PaginacaoDTO<EnderecoDTO>> BuscarEnderecos(PaginacaoDTO<EnderecoDTO> paginacao, string usuarioId);
        Task<int> Salvar(EnderecoPayload item);
    }
}
