using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IEmailEnvioRepository
    {
        Task<int> Salvar(EmailEnvio emailEnvio);
    }
}
