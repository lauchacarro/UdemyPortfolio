using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using UdemyPortfolio.Web.Extensions;

namespace UdemyPortfolio.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfraestructure()
                    .AddOptions(Configuration)
                    .AddServices(Configuration)
                    .AddAuthentication(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseInfraestructure(env)
                .UseAuthentication(env)
                .UseRedirectToProxiedHttps()
                .UseEndpoints();
        }
    }
}
