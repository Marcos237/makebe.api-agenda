using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Entidades.Enum;

namespace api.makebe.agenda.infra.crosscutting.Services.Interfaces
{
    public interface ILogCrossCuttingService
    {
        Task<bool> CriarLog(Log log);
        Task<bool> MontarLog(object Modelo, string mensagem, string metodo, object camposValidados, TipoLog tipoLog,
            string usuario, string request);
        Task<string> ValidarObjetos(object Modelo);
        Task<string> RetornarTipoLog(TipoLog tipoLog);
    }
}
