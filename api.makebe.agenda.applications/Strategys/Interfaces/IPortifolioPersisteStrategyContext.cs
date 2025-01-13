using api.makebe.agenda.applications.Models.Payloads;

namespace api.makebe.agenda.applications.Strategys.Interfaces
{
    public interface IPortifolioPersisteStrategyContext<T> where T : PortifolioPayload
    {
        Task<int> Salvar(T portifolio);
    }
}
