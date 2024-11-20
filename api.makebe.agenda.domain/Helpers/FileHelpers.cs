using Google.Protobuf;

namespace api.makebe.agenda.domain.Helpers
{
    public static class FileHelpers
    {
        public static async Task<string> RetornarArquivo(string caminhoArquivo)
        {
            var conteudoHtml = string.Empty;
            if (!String.IsNullOrEmpty(caminhoArquivo))
                conteudoHtml = await File.ReadAllTextAsync(caminhoArquivo);

            return conteudoHtml;
        }
        public static string CleanBase64String(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                return string.Empty;

            var cleanBase64String = base64String.Split(',')[0];
            base64String = base64String.Replace(cleanBase64String, "").Substring(1);
            return base64String;
        }

        public static byte[] ConvertBaseStringBytes(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                return new byte[0];

            var cleanBase64String = CleanBase64String(base64String);
            var imageBytes = Convert.FromBase64String(cleanBase64String);
            return imageBytes;
        }
        public static bool IsBase64String(string base64)
        {
            if (!base64.Contains("data"))
                return false;
            var urlImagem = CleanBase64String(base64);
            Span<byte> buffer = new Span<byte>(new byte[urlImagem.Length]);
            var retorno = Convert.TryFromBase64String(urlImagem, buffer, out int bytesParsed);
            return retorno;
        }
        public static string GetExtensaoArquivo(string arquivo)
        {
            if (string.IsNullOrEmpty(arquivo))
                return string.Empty;

            return  Path.GetExtension(arquivo).ToLower();
        }
    }
}
