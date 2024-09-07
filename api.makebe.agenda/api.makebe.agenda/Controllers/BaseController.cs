using api.makebe.agenda.applications.Security;
using Microsoft.AspNetCore.Mvc;

namespace api.makebe.agenda.Controllers
{
    public class BaseController : Controller
    {
        public async Task<string> GetIdentity()
        {
            var identity = HttpContext?.User?.Identity;
            if (identity == null)
                return string.Empty;

            var jwtDecode = await IdentityExtensions.DecodificarJWT(identity);
            return jwtDecode?.UsuarioId ?? string.Empty;
        }
    }
}
