using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IEnderecoDomainService
    { 
        Task<PaginacaoDTO<EnderecoDTO>> BuscarTodos(PaginacaoDTO<EnderecoDTO> paginacao, string usuarioId);
        Task<EnderecoDTO> BuscarPorId(int id); 
        Task<int> Salvar(Endereco item);
        Task<Endereco> Atualizar(Endereco item);
        Task<bool> Desativar(int id);

    }
}
