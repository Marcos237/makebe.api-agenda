using Microsoft.AspNetCore.Http;

namespace api.makebe.agenda.applications.Helpers
{
    public static class JwtHelpper
    {
        public static string GetJwtToken(HttpContext httpContext)
        {
            string jwtToken = httpContext.Request.Headers["Authorization"].ToString() ?? string.Empty;
            string token = jwtToken.Replace("Bearer ", "");
            return token;
        }
    }
}
