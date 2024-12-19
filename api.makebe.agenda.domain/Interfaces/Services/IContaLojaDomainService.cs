using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Services
{
    public interface IContaLojaDomainService
    {
        Task<int> Salvar(ContaLoja loja);
    }
}
