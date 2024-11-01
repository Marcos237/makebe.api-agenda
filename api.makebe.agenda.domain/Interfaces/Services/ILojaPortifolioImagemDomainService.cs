using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface ILojaPortifolioImagemDomainService
    {
        Task<IEnumerable<LojaPortifolioImagemDTO>> BuscarImagensPorIdLojaPortifolio(int id);
        Task<LojaPortifolioImagemDTO> BuscarImagensPorId(int id);
        Task<int> Salvar(LojaPortifolioImagens lojaPortifolioImagens);
        Task<bool> Desativar(int id);
    }
}
