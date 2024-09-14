using api.makebe.agenda.applications.Helpers;
using lib.makebe.applications.Security;
using lib.makebe.applications.Services.Interfaces;
using lib.makebe.domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Principal;

namespace api.makebe.agenda.applications.Filters.Authorization
{
    public class AuthorizationFilter : Attribute, IAsyncActionFilter
    {
        public PapeisPermissoes PapeisPermissoes { get; }
        public string? Chave { get; set; }

        public AuthorizationFilter(PapeisPermissoes papeisPermissoes)
        {
            PapeisPermissoes = papeisPermissoes;
            Chave = string.Empty;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var permissoesService = context.HttpContext.RequestServices.GetRequiredService<IPermissaoAutenticacaoService>();
            var chave = JwtHelpper.GetJwtToken(context.HttpContext);
            var identity = context.HttpContext.User?.Identity;
            if (!await IsAuthorized(permissoesService, PapeisPermissoes, identity!, chave))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }
            await next();
        }
        private async Task<bool> IsAuthorized(IPermissaoAutenticacaoService permissoesService, PapeisPermissoes papeisPermissoes,
            IIdentity identity, string chave)
        {
            var claims = (await IdentityExtensions.DecodificarJWT(identity));
            var papeisClaims = claims.Papeis ?? string.Empty;
            var usuarioId = claims.UsuarioId ?? string.Empty;

            var papeis = await permissoesService.ValidarPermissaoAutenticacao(papeisPermissoes, papeisClaims, usuarioId);
            return papeis.Any();
        }
    }
}
