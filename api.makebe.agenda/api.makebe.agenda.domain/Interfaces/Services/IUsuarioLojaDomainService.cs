using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IUsuarioLojaDomainService
    {
        Task<UsuarioLoja> Salvar(UsuarioLoja loja);
    }
}
