using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IArquivoDomainService
    {
        Task<Arquivo> SalvarArquivo(Arquivo arquivo);
        Task<Arquivo> MontarArquivo(string urlImagem, string nomeArquivo);
    }
}
