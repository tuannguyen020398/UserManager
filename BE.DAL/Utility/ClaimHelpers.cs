using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.Utility
{
    public class ClaimHelpers
    {
        public static long GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            Claim claim = claimsPrincipal.Claims.FirstOrDefault((Claim x) => x.Type == "sub" || x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (claim != null)
            {
                long.TryParse(claim.Value, out var result);
                return result;
            }

            return 0L;
        }

    }
}
