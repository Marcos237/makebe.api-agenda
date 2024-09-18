using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Services
{
    public interface IUsuarioLojaDomainService
    {
        Task<int> Salvar(UsuarioLoja loja);
    }
}
