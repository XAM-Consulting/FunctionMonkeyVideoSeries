using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FunctionMonkey.Abstractions;

namespace ServerlessBlog
{
    public class IsApprovedAuthorClaimsAuthorization : IClaimsPrincipalAuthorization
    {
        private static readonly string[] PreapprovedAuthors =
        {
            "I9vuXQT5bh2O7x6O8hgJDPdCXblwb5p8@clients"
        };
        
        public Task<bool> IsAuthorized(
            ClaimsPrincipal claimsPrincipal,
            string httpVerb,
            string requestUrl)
        {
            string subject = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Task.FromResult(subject != null && PreapprovedAuthors.Contains(subject));
        }
    }
}