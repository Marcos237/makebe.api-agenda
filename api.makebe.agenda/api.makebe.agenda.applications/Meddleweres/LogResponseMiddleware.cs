using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.domain.Services.Interfaces;
using api.makebe.agenda.infra.crosscutting.Entidades.Constants;
using api.makebe.agenda.infra.crosscutting.Entidades.Enum;
using api.makebe.agenda.infra.crosscutting.Notifications;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace api.makebe.agenda.applications.Meddleweres
{
    public class LogResponseMiddleware
    {
        private readonly RequestDelegate _next;
        public LogResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var _log = context.RequestServices.GetRequiredService<ILogCrossCuttingService>();
            var _notification = context.RequestServices.GetRequiredService<INotificationContext>();
            var _sessao = context.RequestServices.GetRequiredService<IUsuarioSessaoDomainService>();
            var originalBodyStream = context.Response.Body;
            var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;
            var url = UriHelper.GetDisplayUrl(context.Request);
            var chave = JwtHelpper.GetJwtToken(context);
            var usuario = await _sessao.BuscarSessao(chave);
            var notificationResult = Enumerable.Empty<Notification>();
            var request = context.Request.Method;
            try
            {
                await _next(context);
                responseBodyStream.Seek(0, SeekOrigin.Begin);

                var statusCode = context.Response.StatusCode;
                if (statusCode != StatusCodes.Status204NoContent)
                {
                    var responseBody = new StreamReader(responseBodyStream).ReadToEnd();
                    var mensagem = VerificarTipoMensagem(statusCode);
                    var tipoStatus = VerificarTipoLog(statusCode);

                    notificationResult = VerificarValidacao(_notification.Notifications);
                    await _log.MontarLog(responseBody, mensagem, url, notificationResult, tipoStatus, usuario?.Id.ToString() ?? string.Empty, request);
                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    await responseBodyStream.CopyToAsync(originalBodyStream);
                }
                else
                {
                    await responseBodyStream.CopyToAsync(originalBodyStream);
                }
            }
            catch (Exception ex)
            {
                await _log.MontarLog(ex, ex.Message, url, new List<Notification>(), TipoLog.Error, usuario?.Id.ToString() ?? string.Empty, request);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var respostaErro = ResponseModelHelper<Exception>.RetornarResponseModel(
                    ex, _notification.Notifications
                );

                var respostaErroJson = JsonConvert.SerializeObject(respostaErro);
                await context.Response.WriteAsync(respostaErroJson);
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }

        private string VerificarTipoMensagem(int statusCode)
        {
            if (statusCode == StatusCodes.Status200OK || statusCode == StatusCodes.Status201Created || statusCode == StatusCodes.Status204NoContent)
                return $"{statusCode}/{LogConstant.MensagemSuccess}";

            return $"{statusCode}/{LogConstant.MensagemInformation}";

        }

        private TipoLog VerificarTipoLog(int statusCode)
        {
            if (statusCode == StatusCodes.Status200OK || statusCode == StatusCodes.Status201Created || statusCode == StatusCodes.Status204NoContent)
                return TipoLog.Success;

            return TipoLog.Information;

        }
        private IEnumerable<Notification> VerificarValidacao(IEnumerable<Notification> notifications)
        {
            var notificationsResult = new List<Notification>();
            foreach (var notification in notifications)
            {
                if (notification.IsValidate)
                {
                    notificationsResult.Add(notification);
                }
            }
            return notificationsResult;
        }
    }
}
