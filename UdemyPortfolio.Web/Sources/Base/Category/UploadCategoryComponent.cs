using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using UdemyPortfolio.Models.Validation;
using UdemyPortfolio.Services.Abstracts;

namespace UdemyPortfolio.Web.Sources.Base
{
    public class UploadCategoryComponent : ComponentBase
    {
        [Inject]
        protected ICategoryService CategoryService { get; set; }

        [Parameter]
        public Action OnUploaded { get; set; }

        protected string CategoryName { get; set; }
        protected string ErrorMessage { get; set; }
        protected bool InvalidCategory { get; set; }

        protected async Task Button_OnClick()
        {
            InvalidCategory = false;

            Validation result = await CategoryService.CreateCategoryAsync(new Models.Categories.Category() { Name = CategoryName });
            if (!result.Success)
            {
                InvalidCategory = true;
                ErrorMessage = result.ErrorMessages.First();
            }
            if (!InvalidCategory)
            {
                OnUploaded();
            }
        }
    }
}
