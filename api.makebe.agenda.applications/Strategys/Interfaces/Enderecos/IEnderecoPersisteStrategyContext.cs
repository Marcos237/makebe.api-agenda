using api.makebe.agenda.applications.Models.Payloads;

namespace api.makebe.agenda.applications.Strategys.Interfaces.Enderecos
{
    public interface IEnderecoPersisteStrategyContext<T> where T : EnderecoPayload
    {
        Task<int> Salvar(T portifolio);
    }
}
