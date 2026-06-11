using PesquisarVitrineEvent;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IVitrineService
    {
        Task<List<ItemVitrineResponse>> PesquisarAsync(string valorItem, CancellationToken cancellationToken);
    }
}
