using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.applications.Factorys.Interfaces
{
    public interface IContextFactory<T>
    {
        Task<T> ExecutarService(int tipo);
    }
}
