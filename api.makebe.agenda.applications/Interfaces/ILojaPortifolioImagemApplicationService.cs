using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.applications.Interfaces
{
    public  interface ILojaPortifolioImagemApplicationService
    {
        Task<bool> SalvarImagens(IEnumerable<LojaPortifolioImagemDTO> lojaPortifolioImagens, int lojaPortifolioId);
        Task<bool> ValidarArquivos(IEnumerable<Arquivo> arquivos);

        Task<IEnumerable<LojaPortifolioImagemDTO>> BuscarImagensPorLojaPortifolioId(int lojaPortifolioId);
    }
}
