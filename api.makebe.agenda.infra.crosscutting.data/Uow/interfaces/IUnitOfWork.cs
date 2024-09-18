namespace api.makebe.agenda.infra.data.interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Commit();
        void RollBack();
    }
}
