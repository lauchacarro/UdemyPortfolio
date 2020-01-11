using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using UdemyPortfolio.Models.Settings;

namespace UdemyPortfolio.Web.Extensions
{
    public static class OptionsRegistrationExtensions
    {
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<GitHubSetting>(Configuration.GetSection(nameof(GitHubSetting)));
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
            });

            return services;
        }
    }
}
