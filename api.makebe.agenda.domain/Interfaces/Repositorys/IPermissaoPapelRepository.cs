using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IPermissaoPapelRepository
    {
        Task<PermissaoPapel?> BuscarPorPermissaoId(Guid permissaoId);
    }
}
