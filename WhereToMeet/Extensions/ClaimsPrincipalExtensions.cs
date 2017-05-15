using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WhereToMeet.Middleware.Authentication;

namespace WhereToMeet.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var foundUserIdClaims = claimsPrincipal.Claims.Where(claim => claim.Type == TokenProviderMiddleware.UserIdRegisteredClaimName);
            return Int32.Parse(foundUserIdClaims.First().Value);
        }
    }
}
