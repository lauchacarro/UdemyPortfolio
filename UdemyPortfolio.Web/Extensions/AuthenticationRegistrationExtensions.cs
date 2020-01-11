using System;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UdemyPortfolio.Web.Extensions
{
    public static class AuthenticationRegistrationExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = MicrosoftAccountDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Environment.GetEnvironmentVariable("MS_CLIENT_ID");
                microsoftOptions.ClientSecret = Environment.GetEnvironmentVariable("MS_CLIENT_SECRET");
            });

            return services;
        }
        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }

    }
}
