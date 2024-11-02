using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.applications.Interfaces
{
    public  interface ILojaPortifolioImagemApplicationService
    {
        Task<IEnumerable<LojaPortifolioImagemDTO>> SalvarImagens(IEnumerable<LojaPortifolioImagemDTO> lojaPortifolioImagens);
        Task<bool> ValidarArquivos(IEnumerable<Arquivo> arquivos);
    }
}
