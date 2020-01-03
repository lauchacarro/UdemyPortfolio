using System.Collections.Generic;
using System.Threading.Tasks;

using UdemyPortfolio.Models.GitHub;

namespace UdemyPortfolio.Services.Abstracts
{
    public interface IGitHubService
    {
        Task CreateFileAsync(string path, string fileName, string content);
        Task<FileContent> GetFileContentAsync(string path, string fileName);
        Task<bool> ExistFileAsync(string path, string fileName);
        Task<IEnumerable<FileContent>> GetAllFilesAsync(string path);
    }
}
