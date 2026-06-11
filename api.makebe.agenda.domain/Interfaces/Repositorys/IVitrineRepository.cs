using PesquisarVitrineEvent;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IVitrineRepository
    {
        Task<List<ItemVitrineResponse>> PesquisarAsync(string valorItem, CancellationToken cancellationToken);
    }
}
