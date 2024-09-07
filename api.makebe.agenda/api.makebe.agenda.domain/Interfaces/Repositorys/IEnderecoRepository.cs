using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.infra.data.Repositorys.Interfaces
{
    public interface IEnderecoRepository
    {
        Task<IEnumerable<Endereco>> BuscarEnderecos(PaginacaoDTO<Endereco> paginacao,  string usuarioId);
        Task<Endereco> BuscarPorId(int id);
        Task<int> Salvar(Endereco endereco);
        Task<Endereco> Atualizar(Endereco endereco);
        Task<bool>Deastivar(int id);
    }
}
