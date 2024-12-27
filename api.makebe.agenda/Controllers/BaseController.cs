using lib.makebe.applications.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.makebe.agenda.Controllers
{
    public class BaseController : Controller
    {
        public string? Chave { get; private set; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Chave = await GetIdentity() ?? string.Empty;
            await next();
        }

        [NonAction]
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
