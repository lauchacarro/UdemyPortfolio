﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using UdemyPortfolio.Services.Abstracts;
using UdemyPortfolio.Services.Concretes;

namespace UdemyPortfolio.Web.Extensions
{
    public static class ServicesRegistrationExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IGitHubService, GitHubService>();
            services.AddSingleton<IUdemyService, UdemyService>();
            services.AddSingleton<ICertificateService, CertificateService>();
            services.AddSingleton<ICertificateExcelService, CertificateExcelService>();
            services.AddSingleton<IPathService, PathService>();
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
