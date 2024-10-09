namespace api.makebe.agenda.infra.data.interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransaction();
        void Commit();
        void Rollback();
    }
}
