using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.infra.data.Repositorys.Interfaces
{
    public interface IUsuarioLojaRepository
    {
        Task<int> Salvar(UsuarioLoja loja);
    }
}
