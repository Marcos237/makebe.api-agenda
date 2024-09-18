using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces
{
    public interface IUsuarioLojaDomainService
    {
        Task<UsuarioLoja> Salvar(UsuarioLoja loja);
    }
}
