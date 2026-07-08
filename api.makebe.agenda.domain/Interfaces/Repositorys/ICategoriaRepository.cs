using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface ICategoriaRepository
    {
        Task<int> Salvar(Categoria categoria);
        Task<IEnumerable<Categoria>> BuscarPorServico(int servicoId);
        Task<bool> DesativarPorServico(int servicoId);
    }
}
