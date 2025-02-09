using System.Security.Claims;

namespace Buoi2.Models
{
    public static class ClaimStore
    {
        public static List<Claim> GetAllClaims()
        {
            return new List<Claim>()
            {
                new Claim("Create","Create"),
                new Claim("Edit","Edit"),
                new Claim("Delete","Delete")
            };
        }
    }
}
