using System.Collections.Generic;
using System.Threading.Tasks;

using UdemyPortfolio.Models.Categories;
using UdemyPortfolio.Models.Validation;

namespace UdemyPortfolio.Services.Abstracts
{
    public interface ICategoryService
    {
        IAsyncEnumerable<Category> GetCategoriesAsync();
        Task<Category> GetCategoryAsync(string categoryName);
        IAsyncEnumerable<Category> GetCategoriesAsync(string identifier);
        Task<Validation> DeleteCategoryAsync(Category category);
        Task<Validation> UpdateCategoryAsync(Category category);
        Task<Validation> CreateCategoryAsync(Category category);
    }
}
