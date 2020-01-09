using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using UdemyPortfolio.Models.Udemy;
using UdemyPortfolio.Models.Validation;
using UdemyPortfolio.Services.Abstracts;

namespace UdemyPortfolio.Web.Shared.Base
{
    public class AdminLayoutComponent : LayoutComponentBase
    {

        [Inject]
        protected NavigationManager Navigation { get; set; }
        [Inject]
        protected ICertificateService CertificateService { get; set; }
        [Inject]
        protected IPathService PathService { get; set; }

        public User User { get; set; }
        public string NoUserMessage { get; set; }
        public string Identifier { get; set; }
        protected bool Loading { get; set; } = true;
        protected bool Retry { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender || Retry)
            {
                if (Loading)
                {
                    ValidationResult<User> user = await CertificateService.GetUserAsync();
                    if (user.Success)
                    {
                        User = user.Result;
                        Identifier = PathService.GetUserIdentifier();
                        Navigation.NavigateTo("/Admin/Certificates");

                    }
                    else
                    {
                        NoUserMessage = user.ErrorMessages.FirstOrDefault();
                        Navigation.NavigateTo("/Admin/FirstCertificate");
                    }
                }

                Loading = false;
                StateHasChanged();
            }
        }

        public void LoadingAgain()
        {
            Retry = Loading = true;
            StateHasChanged();
        }
    }
}
