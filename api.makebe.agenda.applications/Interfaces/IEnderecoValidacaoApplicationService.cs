using api.makebe.agenda.applications.Models.Payloads;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IEnderecoValidacaoApplicationService
    {
        Task<bool>Validar(EnderecoPayload enderecoPayload);
        void RetornarListaVazia(string entidade, string mensagem);
    }
}
