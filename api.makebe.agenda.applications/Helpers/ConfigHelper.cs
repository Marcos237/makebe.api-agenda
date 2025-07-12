using Microsoft.Extensions.Configuration;

namespace api.makebe.agenda.applications.Helpers
{
    public static class ConfigHelper
    {
        private static IConfiguration? _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string? GetValue(string chave)
        {
            return _configuration?[chave] ?? string.Empty;
        }
    }
}
