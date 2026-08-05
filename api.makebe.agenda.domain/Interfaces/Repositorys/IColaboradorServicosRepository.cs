using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IColaboradorServicosRepository
    {
        Task<int> Salvar(ColaboradorServicos colaboradorServico);
        Task<ColaboradorServicos> BuscarPorId(int id);
        Task<IEnumerable<ColaboradorServicos>> BuscarPorColaboradorId(int colaboradorId);
        Task<bool> Atualizar(ColaboradorServicos colaboradorServico);
        Task<bool> Remover(int id);
        Task<bool> RemoverPorColaboradorEServico(int colaboradorId, int servicoId);
        Task<bool> RemoverTodosPorColaborador(int colaboradorId);
    }
}
