using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Strategys.Interfaces.Enderecos
{
    public interface IEnderecoBuscaStrategy
    {
        Task<PaginacaoDTO<EnderecoDTO>> BuscarEnderecos(PaginacaoDTO<EnderecoDTO> paginacao, string usuarioId);
    }
}
