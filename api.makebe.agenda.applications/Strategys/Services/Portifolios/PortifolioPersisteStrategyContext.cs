using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Strategys.Interfaces;

namespace api.makebe.agenda.applications.Strategys.Services.Portifolios
{
    public class PortifolioPersisteStrategyContext<T> : IPortifolioPersisteStrategyContext<T> where T : PortifolioPayload
    {
        private readonly IEnumerable<IPortifolioPersisteStrategy<PortifolioPayload>> _portifolioPersisteStrategies;
        public PortifolioPersisteStrategyContext(IEnumerable<IPortifolioPersisteStrategy<PortifolioPayload>> portifolioPersisteStrategies)
        {
            _portifolioPersisteStrategies = portifolioPersisteStrategies;
        }
        public async Task<int> Salvar(T portifolio)
        {
            var itemSalvo = 0;
            foreach (var item in _portifolioPersisteStrategies)
            {
                itemSalvo = await item.Salvar(portifolio);
            }
            return itemSalvo;
        }
    }
}
