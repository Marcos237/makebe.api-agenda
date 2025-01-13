using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IPortifolioDomainService
    {
        Task<PortifolioDTO> BuscarPorId(int id);
        Task<int> Salvar(Portifolio portifolio);
        Task<Portifolio> Atualizar(Portifolio portifolio);
        Task<bool> Deastivar(int id);
    }
}
