using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pizza_Demo.Utilities
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetId(this ClaimsPrincipal self)
        {
            return self.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static bool IsAdmin(this ClaimsPrincipal self)
        {
            return self.IsInRole("Administrator");
        }
    }
}
