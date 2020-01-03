using System;
using System.IO;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using UdemyPortfolio.Models.Settings;
using UdemyPortfolio.Services.Abstracts;

namespace UdemyPortfolio.Services.Concretes
{
    public class PathService : IPathService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GitHubSetting _githubSetting;

        public PathService(IHttpContextAccessor httpContextAccessor, IOptions<GitHubSetting> gitHubSettingOptions)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _githubSetting = gitHubSettingOptions?.Value ?? throw new ArgumentNullException(nameof(gitHubSettingOptions));
        }

        public string GetCertificateFolder()
        {
            string identifierPath = GetUserFolder();
            return Path.Combine(identifierPath, "certificates");
        }

        public string GetCertificateFolder(string identifier)
        {
            string identifierPath = GetUserFolder(identifier);
            return Path.Combine(identifierPath, "certificates");
        }

        public string GetUserFolder()
        {
            string identifier = GetUserIdentifier();
            string identifierPath = Path.Combine(_githubSetting.IdentifierFolderPath, identifier);
            return identifierPath;
        }

        public string GetUserFolder(string identifier)
        {
            string identifierPath = Path.Combine(_githubSetting.IdentifierFolderPath, identifier);
            return identifierPath;
        }

        public string GetUserIdentifier()
        {
            string identifier = _httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return identifier;
        }
    }
}
