using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.infra.data.Repositorys.Interfaces
{
    public interface IEnderecoRepository
    {
        Task<PaginacaoDTO<EnderecoDTO>> BuscarEnderecos(PaginacaoDTO<EnderecoDTO> paginacao,  string usuarioId);
        Task<EnderecoDTO> BuscarPorId(int id);
        Task<int> Salvar(Endereco endereco);
        Task<Endereco> Atualizar(Endereco endereco);
        Task<bool>Deastivar(int id);
    }
}
