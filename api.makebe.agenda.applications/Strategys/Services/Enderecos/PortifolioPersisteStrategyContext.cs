using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Strategys.Interfaces.Enderecos;

namespace api.makebe.agenda.applications.Strategys.Services.Portifolios
{
    public class EnderecoPersisteStrategyContext<T> : IEnderecoPersisteStrategyContext<T> where T : EnderecoPayload
    {
        private readonly IEnumerable<IEnderecoPersisteStrategy<EnderecoPayload>> _enderecoPersisteStrategies;
        public EnderecoPersisteStrategyContext(IEnumerable<IEnderecoPersisteStrategy<EnderecoPayload>> enderecoPersisteStrategies)
        {
            _enderecoPersisteStrategies = enderecoPersisteStrategies;
        }
        public async Task<int> Salvar(T portifolio)
        {
            var itemSalvo = 0;
            foreach (var item in _enderecoPersisteStrategies)
            {
                itemSalvo = await item.Salvar(portifolio);
            }
            return itemSalvo;
        }
    }
}
