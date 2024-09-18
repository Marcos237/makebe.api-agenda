using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Entidades.Enum;
using api.makebe.agenda.infra.crosscutting.Repositorys.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using Newtonsoft.Json;

namespace api.makebe.agenda.infra.crosscutting.Services
{
    public class LogCrossCuttingService : ILogCrossCuttingService
    {
        private readonly ILogRepository _repository;

        public LogCrossCuttingService(ILogRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> CriarLog(Log log)
        {
            return await _repository.Gravarlog(log);
        }

        public async Task<bool> MontarLog(object Modelo, string mensagem, string metodo,
            object camposValidados, TipoLog tipoLog, string usuario, string request)
        {

            var objeto = await ValidarObjetos(Modelo);
            var campos = await ValidarObjetos(camposValidados);
            var tipo = await RetornarTipoLog(tipoLog);

            var log = new Log()
            {
                Metodo = metodo,
                Mensagem = mensagem,
                Objeto = objeto,
                DataCadastro = DateTime.Now,
                CamposValidados = campos,
                Tipo = tipo,
                Usuario = usuario,
                Request = request
            };

            return await CriarLog(log);
        }
        public async Task<string> ValidarObjetos(object Modelo)
        {
            var objeto = "";
            if (Modelo != null)
                objeto = JsonConvert.SerializeObject(Modelo);

            else
                return "";

            return await Task.FromResult(objeto);

        }
        public async Task<string> RetornarTipoLog(TipoLog tipoLog)
        {
            var retorno = "";

            switch (tipoLog)
            {
                case TipoLog.Error:
                    retorno = "Error";
                    return await Task.FromResult(retorno);

                case TipoLog.Information:
                    retorno = "Information";
                    return await Task.FromResult(retorno);

                case TipoLog.Validation:
                    retorno = "Validation";
                    return await Task.FromResult(retorno);

                case TipoLog.Connection:
                    retorno = "Connection";
                    return await Task.FromResult(retorno);

                case TipoLog.Security:
                    retorno = "Security";
                    return await Task.FromResult(retorno);

                case TipoLog.Success:
                    retorno = "Success";
                    return await Task.FromResult(retorno);
                default:
                    return retorno;
            }
        }


    }
}