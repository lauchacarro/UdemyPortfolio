using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using UdemyPortfolio.Models.Categories;
using UdemyPortfolio.Services.Abstracts;

namespace UdemyPortfolio.Web.Sources.Base
{
    public class DeleteCategoryComponent : ComponentBase
    {
        [Inject]
        protected ICategoryService CategoryService { get; set; }

        [Parameter]
        public Category Category { get; set; }
        [Parameter]
        public Action<Category> OnDeleted { get; set; }

        protected async Task Button_OnClick()
        {
            await CategoryService.DeleteCategoryAsync(Category);
            OnDeleted(Category);
        }
    }
}
