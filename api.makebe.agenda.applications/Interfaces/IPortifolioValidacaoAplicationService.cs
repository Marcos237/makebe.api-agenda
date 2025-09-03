using api.makebe.agenda.applications.Models.Payloads;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IPortifolioValidacaoAplicationService
    {
        Task<bool> Validar(PortifolioPayload portifolioPayload);

        void RetornarListaVazia(string entidade, string mensagem);
    }
}
