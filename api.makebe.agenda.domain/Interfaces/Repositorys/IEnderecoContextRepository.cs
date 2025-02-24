namespace api.makebe.agenda.infra.data.Repositorys
{
    public interface IEnderecoContextRepository<TSalvar, TBuscar> where TSalvar : class where TBuscar : class
    {
        Task<IEnumerable<TBuscar>> BuscarEnderecos(string contaId);
        Task<int> Salvar(TSalvar item);
        Task<bool> Atualizar(TSalvar item);
    }
}