using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IPortifolioContextRepository<TSalvar, TBuscar> where TSalvar : class where TBuscar : class
    {
        Task<IEnumerable<TBuscar>> BuscarPortifolios(string contaId);
        Task<int> Salvar(TSalvar item);
        Task<bool> Atualizar(TSalvar item);
    }
}
