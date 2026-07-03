using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Services;
using lib.makebe.applications.Security;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace api.makebe.agenda.applications.Services.Autenticacao
{
    public class UsuarioAutenticadoService : IUsuarioAutenticadoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioAutenticadoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UsuarioAutenticadoDTO> BuscarUsuarioAutenticado()
        {
            var identity = _httpContextAccessor.HttpContext?.User?.Identity;
            if (identity == null || !identity.IsAuthenticated)
                return new UsuarioAutenticadoDTO();

            var claims = await IdentityExtensions.DecodificarJWT(identity);

            var response = new UsuarioAutenticadoDTO
            {
                UsuarioId = PropiedadesHelper.ParseGuidOrDefault(claims.UsuarioId),
                PermissaoId = PropiedadesHelper.ParseGuidOrDefault(BuscarValorPermissao(claims))
            };
            return response;
        }

        private static string BuscarValorPermissao(object claims)
        {
            var propriedadesPermissao = new[] { "Value", "permissao" };

            foreach (var propriedade in propriedadesPermissao)
            {
                var valor = claims.GetType()
                    .GetProperty(propriedade, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)?
                    .GetValue(claims)?
                    .ToString();

                if (!string.IsNullOrWhiteSpace(valor))
                    return valor;
            }

            return string.Empty;
        }
    }
}
