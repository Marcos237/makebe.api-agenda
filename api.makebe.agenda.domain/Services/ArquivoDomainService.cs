using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace api.makebe.agenda.domain.Services
{
    public class ArquivoDomainService : IArquivoDomainService
    {
        private readonly IConfiguration _configuration;
        private readonly IValidator<Arquivo> _validator;
        private readonly INotificationContext _notificationContext;
        private readonly string _path;
        public ArquivoDomainService(IConfiguration configuration, IValidator<Arquivo> validator, INotificationContext notificationContext)
        {
            _configuration = configuration;
            _validator = validator;
            _path = _configuration["uploads"] ?? string.Empty;
            _notificationContext = notificationContext;
        }

        public async Task<Arquivo> MontarArquivo(string urlImagem, string nomeArquivo)
        {
            var base6Imagem = urlImagem;
            if (!FileHelpers.IsBase64String(base6Imagem))
                return new Arquivo { NomeArquivo = nomeArquivo, UrlImagem = urlImagem };

            if (String.IsNullOrEmpty(nomeArquivo)) return new Arquivo();
            string extensao = Path.GetExtension(nomeArquivo).ToLower();
            var nomeSplit = nomeArquivo.Split('.');
            var nomeArquivoCustomer = $"{nomeSplit[0]}_{Guid.NewGuid()}_{DateTime.Now.ToString("dd-MM-yyyy")}{extensao}";
            var arquivo = new Arquivo()
            {
                UrlImagem = $"{_configuration[BaseConstant.urlImagens]}{nomeArquivoCustomer}",
                NomeArquivo = nomeArquivoCustomer,
                TipoArquivo = extensao,
                ArquivoBase64 = base6Imagem
            };

            return await SalvarArquivo(arquivo);
        }

        public async Task<Arquivo> SalvarArquivo(Arquivo arquivo)
        {
            var arquvoValido = await _validator.ValidateAsync(arquivo);
            if (!arquvoValido.IsValid)
            {
                arquvoValido.Errors.ForEach(x =>
                {
                    _notificationContext.AddNotification(nameof(Arquivo),
                    x.ErrorMessage.ToString());
                });
                return new Arquivo();
            }
            if (!String.IsNullOrEmpty(arquivo.UrlImagem))
            {
                var caminho = Path.Combine(_path, arquivo.NomeArquivo!);
                var baseLimpo = FileHelpers.CleanBase64String(arquivo?.ArquivoBase64 ?? string.Empty);
                byte[] imageBytes = Convert.FromBase64String(baseLimpo);
                File.WriteAllBytes(caminho, imageBytes!);
                arquivo!.UrlImagem = caminho ?? string.Empty;
            }
            return arquivo;
        }
    }
}
