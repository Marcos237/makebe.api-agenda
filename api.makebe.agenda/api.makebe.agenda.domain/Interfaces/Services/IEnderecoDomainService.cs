using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IEnderecoDomainService
    { 
        Task<IEnumerable<Endereco>> BuscarTodos(PaginacaoDTO<Endereco> paginacao, string usuarioId);
        Task<IEnumerable<Endereco>> BuscarPorLojaId(int id);
        Task<Endereco> BuscarPorId(int id); 
        Task<int> Salvar(Endereco item);
        Task<Endereco> Atualizar(Endereco item);
        Task<bool> Desativar(int id);

    }
}
