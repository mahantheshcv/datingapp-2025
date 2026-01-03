using System;
using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimPrincipalExtensions
    {
        public static int GetMemberId(this ClaimsPrincipal user)
        {
            var memberIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(memberIdValue))
                throw new Exception("Cannot get memberId from token");

            if (!int.TryParse(memberIdValue, out var memberId))
                throw new Exception("Invalid memberId in token");

            return memberId;
        }
    }
}
