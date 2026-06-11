using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IPortifolioImagemRepository
    {
        Task<IEnumerable<PortifolioImagemDTO>> BuscarImagensPorIdPortifolio(int id);
        Task<PortifolioImagemDTO> BuscarImagensPorId(int id);
        Task<IEnumerable<ColaboradorPortifolioImagemDTO>> BuscarImagensPorColaboradorId(int id);
        Task<int> Salvar(PortifolioImagens lojaPortifolioImagens);
        Task<PortifolioImagens> Atualizar(PortifolioImagens lojaPortifolioImagens);
        Task<bool> Desativar(int id);
    }
}
