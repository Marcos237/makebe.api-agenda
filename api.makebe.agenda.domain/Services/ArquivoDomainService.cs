using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using Microsoft.Extensions.Configuration;

namespace api.makebe.agenda.domain.Services
{
    public class ArquivoDomainService : IArquivoDomainService
    {
        private readonly IConfiguration _configuration;
        private readonly INotificationContext _notificationContext;
        private readonly string _path;
        public ArquivoDomainService(IConfiguration configuration, INotificationContext notificationContext)
        {
            _configuration = configuration;
            _path = _configuration["uploads"] ?? string.Empty;
            _notificationContext = notificationContext;
        }

        public async Task<Arquivo> MontarArquivo(string urlImagem, string nomeArquivo)
        {
            var base6Imagem = urlImagem;
            if (!FileHelpers.IsBase64String(base6Imagem))
                return new Arquivo { NomeArquivo = nomeArquivo, UrlImagem = urlImagem };

            if (String.IsNullOrEmpty(nomeArquivo)) return new Arquivo();
            string extensao = FileHelpers.GetExtensaoArquivo(nomeArquivo);
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
            if (!String.IsNullOrEmpty(arquivo.UrlImagem))
            {
                var caminho = Path.Combine(_path, arquivo.NomeArquivo!);
                var baseLimpo = FileHelpers.CleanBase64String(arquivo?.ArquivoBase64 ?? string.Empty);
                byte[] imageBytes = Convert.FromBase64String(baseLimpo);
                await File.WriteAllBytesAsync(caminho, imageBytes!);
            }
            return arquivo!;
        }
    }
}
