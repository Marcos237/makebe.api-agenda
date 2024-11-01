using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface ILojaPortifolioImagemRepository
    {
        Task<IEnumerable<LojaPortifolioImagemDTO>> BuscarImagensPorIdLojaPortifolio(int id);
        Task<LojaPortifolioImagemDTO> BuscarImagensPorId(int id);
        Task<int> Salvar(LojaPortifolioImagens lojaPortifolioImagens);
        Task<LojaPortifolioImagens> Atualizar(LojaPortifolioImagens lojaPortifolioImagens);
        Task<bool> Desativar(int id);
    }
}
