using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.applications.Interfaces
{
    public  interface IPortifolioImagemApplicationService
    {
        Task<bool> SalvarImagens(IEnumerable<PortifolioImagemDTO> lojaPortifolioImagens, int lojaPortifolioId);
        Task<bool> ValidarArquivos(IEnumerable<Arquivo> arquivos);

        Task<IEnumerable<PortifolioImagemDTO>> BuscarImagensPorLojaPortifolioId(int lojaPortifolioId);
    }
}
