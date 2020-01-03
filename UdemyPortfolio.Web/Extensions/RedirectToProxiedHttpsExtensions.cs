using Microsoft.AspNetCore.Rewrite;

using UdemyPortfolio.Web.Rules;

namespace UdemyPortfolio.Web.Extensions
{
    public static class RedirectToProxiedHttpsExtensions
    {
        public static RewriteOptions AddRedirectToProxiedHttps(this RewriteOptions options)
        {
            options.Rules.Add(new RedirectToProxiedHttpsRule());
            return options;
        }
    }
}
