using api.makebe.agenda.infra.data.interfaces;
using api.makebe.agenda.infra.data.Repositorys;

namespace api.makebe.agenda.infra.data.Uow.interfaces
{
    public sealed class UnitOfWork : IUnitOfWork
    {

        private readonly DbAgenda _agenda;

        public UnitOfWork(DbAgenda agenda)
        {
            _agenda = agenda;
        }

        public async Task BeginTransaction()
        {
            _agenda.Transaction = await Task.FromResult(_agenda.Connection.BeginTransaction());
        }

        public void Commit()
        {
            _agenda?.Transaction?.Commit();
            Dispose();
        }

        public void Rollback()
        {
            _agenda?.Transaction?.Rollback();
            Dispose();
        }

        public  void Dispose() => _agenda.Transaction?.Dispose();
    }
}
