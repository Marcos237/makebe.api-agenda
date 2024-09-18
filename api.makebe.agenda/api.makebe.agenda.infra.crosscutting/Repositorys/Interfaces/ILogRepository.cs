using api.makebe.agenda.infra.crosscutting.Entidades;

namespace api.makebe.agenda.infra.crosscutting.Repositorys.Interfaces
{
    public interface ILogRepository
    {
        Task<bool> Gravarlog(Log log);
    }
}
