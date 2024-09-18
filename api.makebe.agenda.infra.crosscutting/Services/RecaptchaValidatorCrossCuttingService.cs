using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace api.makebe.agenda.infra.crosscutting.Services
{
    public class RecaptchaValidatorCrossCuttingService : IRecaptchaValidatorCrossCuttingService
    {
        private readonly string recaptchaSecretKey;
        private readonly string urlGoogleRecaptcha;
        private readonly IConfiguration _configuration;
        public RecaptchaValidatorCrossCuttingService(IConfiguration configuration)
        {
            _configuration = configuration;
            recaptchaSecretKey = _configuration["recaptchaSecretKey"] ?? string.Empty;
            urlGoogleRecaptcha = _configuration["urlGoogleRecaptcha"] ?? string.Empty;
        }
        public async Task<RespostaRecaptcha> ValidarRecaptcha(string chave)
        {
            using (var httpClient = new HttpClient())
            {
                var postData = new Dictionary<string, string> { { "secret", recaptchaSecretKey }, { "response", chave } };
                var content = new FormUrlEncodedContent(postData);
                var resposta = await httpClient.PostAsync(urlGoogleRecaptcha, content);
                resposta.EnsureSuccessStatusCode();
                var respostaJson = await resposta.Content.ReadAsStringAsync();
                var respostaRecaptcha = JsonConvert.DeserializeObject<RespostaRecaptcha>(respostaJson)
                    ?? new RespostaRecaptcha();

                return respostaRecaptcha;
            }
        }

    }
}
