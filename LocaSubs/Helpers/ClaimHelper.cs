using System.Security.Claims;

namespace LocaSubs.Helpers;

public static class ClaimHelper
{
    public static bool GetUserLogin(HttpContext httpContext, out string login)
    {
        var userLogin = httpContext.User.Claims
            .FirstOrDefault(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType)
            ?.Value;

        login = userLogin!;

        return login is not null;
    }
}