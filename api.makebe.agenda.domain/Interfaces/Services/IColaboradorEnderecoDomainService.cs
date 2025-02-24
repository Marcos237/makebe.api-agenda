using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IColaboradorEnderecoDomainService
    {
        Task<PaginacaoDTO<EnderecoDTO>> BuscarEndereco(PaginacaoDTO<EnderecoDTO> paginacao, string contaId, IEnumerable<UsuarioDTO> usuarios);
        Task<int> Salvar(ColaboradorEndereco item);
        Task<PaginacaoDTO<EnderecoDTO>> MontarColaborador(PaginacaoDTO<EnderecoDTO> paginacao, IEnumerable<UsuarioDTO> usuarios);
        Task<PaginacaoDTO<EnderecoDTO>> Filtrar(PaginacaoDTO<EnderecoDTO> paginacao);
    }
}
