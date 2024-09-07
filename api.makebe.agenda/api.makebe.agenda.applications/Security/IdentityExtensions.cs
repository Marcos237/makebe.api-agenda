using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Entidades;
using System.Security.Claims;
using System.Security.Principal;

namespace api.makebe.agenda.applications.Security
{

    public static class IdentityExtensions
    {
        public static async Task<JWTDecode> DecodificarJWT(this IIdentity identity)
        {
            var claimsIdentity = (ClaimsIdentity)identity;
            var idClaim = claimsIdentity?.FindFirst(x => x.Type == AutenticacaoServiceConstant.tipoClaim)?.Value;
            var descricaoClaim = claimsIdentity?.FindFirst(AutenticacaoServiceConstant.tipoClaim)?.Value;
            var papeisClaim = claimsIdentity?.FindFirst(AutenticacaoServiceConstant.papeis)?.Value;
            var usuarioId = claimsIdentity?.FindFirst(AutenticacaoServiceConstant.Id)?.Value;
            return  await Task.FromResult(new JWTDecode { key = idClaim, Value = descricaoClaim, Papeis = papeisClaim, UsuarioId = usuarioId });
        }
    }
}
