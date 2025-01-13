using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IPortifolioImagemDomainService
    {
        Task<IEnumerable<PortifolioImagemDTO>> BuscarImagensPorIdPortifolio(int id);
        Task<PortifolioImagemDTO> BuscarImagensPorId(int id);
        Task<int> Salvar(PortifolioImagens lojaPortifolioImagens);
        Task<bool> Desativar(int id);
    }
}
