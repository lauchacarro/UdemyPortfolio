using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using Octokit;

using RestSharp;

using UdemyPortfolio.Models.GitHub;
using UdemyPortfolio.Models.Settings;
using UdemyPortfolio.Services.Abstracts;

namespace UdemyPortfolio.Services.Concretes
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubSetting _githubSetting;

        public GitHubService(IOptions<GitHubSetting> githubSetting)
        {
            _githubSetting = githubSetting.Value;
            _client = new GitHubClient(new ProductHeaderValue(nameof(UdemyPortfolio)));
            Credentials basicAuth = new Credentials(Environment.GetEnvironmentVariable("GITHUB_TOKEN"));


            _client.Credentials = basicAuth;
            _githubSetting = githubSetting.Value;
        }
        public async Task CreateFileAsync(string path, string fileName, string content)
        {
            await _client.Repository.Content.CreateFile(
                    _githubSetting.RepositoryID,
                    Path.Combine(path, fileName),
                    new CreateFileRequest($"Create {fileName}",
                                          content,
                                          _githubSetting.Branch));
        }

        public async Task DeleteFileAsync(string path, string fileName)
        {
            IReadOnlyList<RepositoryContent> contents = await _client.Repository.Content.GetAllContents(_githubSetting.RepositoryID, Path.Combine(path, fileName));
            RepositoryContent repositoryContent = contents.First();

            await _client.Repository.Content.DeleteFile(
                _githubSetting.RepositoryID,
                    Path.Combine(path, fileName),
                    new DeleteFileRequest($"Delete {fileName}",
                                          repositoryContent.Sha,
                                          _githubSetting.Branch)
                    );
        }

        public async Task UpdateFileAsync(string path, string fileName, string content)
        {
            IReadOnlyList<RepositoryContent> contents = await _client.Repository.Content.GetAllContents(_githubSetting.RepositoryID, Path.Combine(path, fileName));
            RepositoryContent repositoryContent = contents.First();

            await _client.Repository.Content.UpdateFile(
                _githubSetting.RepositoryID,
                    Path.Combine(path, fileName),
                    new UpdateFileRequest($"Update {fileName}",
                                          content,
                                          repositoryContent.Sha,
                                          _githubSetting.Branch)
                    );
        }

        public async Task<bool> ExistFileAsync(string path, string fileName)
        {
            FileContent fileContent = await GetFileContentAsync(path, fileName);
            return fileContent != null;
        }

        public async Task<IEnumerable<FileContent>> GetAllFilesAsync(string path)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                IReadOnlyList<RepositoryContent> contents = await _client.Repository.Content.GetAllContents(_githubSetting.RepositoryID, path);

                var files = contents.Where(x => x.Type == new StringEnum<ContentType>(ContentType.File)).Select(x => new FileContent()
                {
                    Name = x.Name,
                    Content = x.Content,
                    DownloadUrl = x.DownloadUrl
                });

                return files;
            }
            catch (Octokit.NotFoundException)
            {
                return null;
            }
        }

        public async Task<FileContent> GetFileContentAsync(string path, string fileName)
        {
            try
            {
                IReadOnlyList<RepositoryContent> contents = await _client.Repository.Content.GetAllContents(_githubSetting.RepositoryID, Path.Combine(path, fileName));
                RepositoryContent content = contents.First();
                return new FileContent()
                {
                    Name = content.Name,
                    Content = content.Content,
                    DownloadUrl = content.DownloadUrl
                };
            }
            catch (Octokit.NotFoundException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<FileContent>> GetAllFilesWithContentAsync(string path)
        {
            try
            {
                IReadOnlyList<RepositoryContent> contents = await _client.Repository.Content.GetAllContents(_githubSetting.RepositoryID, path);

                var files = contents.Where(x => x.Type == new StringEnum<ContentType>(ContentType.File)).Select(x => new FileContent()
                {
                    Name = x.Name,
                    Content = x.Content,
                    DownloadUrl = x.DownloadUrl
                }).ToList();

                for (int i = 0; i < files.Count(); i++)
                {

                    RestClient client = new RestClient();
                    RestRequest request = new RestRequest(files[i].DownloadUrl);
                    files[i].Content = client.Get(request).Content;
                }

                return files;
            }
            catch (Octokit.NotFoundException)
            {
                return null;
            }
        }
    }
}