using api.makebe.agenda.infra.data.interfaces;
using api.makebe.agenda.infra.data.Repositorys;
using System.Data;

namespace api.makebe.agenda.infra.data.Uow.interfaces
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbAgenda _agenda;
        private bool _transactionStarted;

        public UnitOfWork(DbAgenda session)
        {
            _agenda = session;
        }

        public void BeginTransaction()
        {
            if (_agenda.Transaction == null)
            {
                _agenda.Transaction = _agenda.Connection.BeginTransaction();
                _transactionStarted = true;
            }
        }

        public void Commit()
        {
            if (_transactionStarted)
            {
                _agenda?.Transaction?.Commit();
                _transactionStarted = false;
            }
        }

        public void RollBack()
        {
            if (_transactionStarted)
            {
                _agenda?.Transaction?.Rollback();
                _transactionStarted = false;
            }
        }

        public void Dispose()
        {
            if (_agenda.Transaction != null)
            {
                _agenda.Transaction.Dispose();
                _transactionStarted = false;
            }
            if (_agenda.Connection.State == ConnectionState.Open)
            {
                _agenda.Connection.Close();
                _agenda.Connection.Dispose();
            }
        }
    }
}
