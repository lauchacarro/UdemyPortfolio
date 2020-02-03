using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

using UdemyPortfolio.Models.Categories;
using UdemyPortfolio.Models.GitHub;
using UdemyPortfolio.Models.Validation;
using UdemyPortfolio.Services.Abstracts;

namespace UdemyPortfolio.Services.Concretes
{
    public class CategoryService : ICategoryService
    {
        private readonly IGitHubService _githubService;
        private readonly IPathService _pathService;

        public CategoryService(IGitHubService githubService, IPathService pathService)
        {
            _githubService = githubService;
            _pathService = pathService;
        }

        public async IAsyncEnumerable<Category> GetCategoriesAsync()
        {
            string categoryPath = _pathService.GetCategoryFolder();
            IEnumerable<FileContent> categories = await _githubService.GetAllFilesWithContentAsync(categoryPath) ?? new List<FileContent>();

            foreach (FileContent file in categories)
            {
                yield return JsonSerializer.Deserialize<Category>(file.Content);
            }
        }

        public async IAsyncEnumerable<Category> GetCategoriesAsync(string identifier)
        {
            string categoryPath = _pathService.GetCategoryFolder(identifier);
            IEnumerable<FileContent> categories = await _githubService.GetAllFilesWithContentAsync(categoryPath) ?? new List<FileContent>();

            foreach (FileContent file in categories)
            {
                yield return JsonSerializer.Deserialize<Category>(file.Content);
            }
        }

        public async Task<Validation> DeleteCategoryAsync(Category category)
        {
            Validation validation = new Validation();
            string categoryPath = _pathService.GetCategoryFolder();
            await _githubService.DeleteFileAsync(categoryPath, category.Name);

            return validation;
        }

        public async Task<Validation> CreateCategoryAsync(Category category)
        {
            Validation validation = new Validation();
            string categoryPath = _pathService.GetCategoryFolder();
            string content = JsonSerializer.Serialize(category);
            await _githubService.CreateFileAsync(categoryPath, category.Name, content);

            return validation;
        }

        public async Task<Category> GetCategoryAsync(string categoryName)
        {
            string categoryPath = _pathService.GetCategoryFolder();
            FileContent file = await _githubService.GetFileContentAsync(categoryPath, categoryName);
            Category category = JsonSerializer.Deserialize<Category>(file.Content);

            return category;
        }

        public async Task<Validation> UpdateCategoryAsync(Category category)
        {
            Validation validation = new Validation();
            string categoryPath = _pathService.GetCategoryFolder();
            string content = JsonSerializer.Serialize(category);
            await _githubService.UpdateFileAsync(categoryPath, category.Name, content);

            return validation;
        }
    }
}
