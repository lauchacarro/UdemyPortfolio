using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using UdemyPortfolio.Models;
using UdemyPortfolio.Models.Validation;
using UdemyPortfolio.Services.Abstracts;

namespace UdemyPortfolio.Web.Components
{
    public class ProfileComponent : ComponentBase
    {
        [Inject]
        protected NavigationManager Navigation { get; set; }
        [Inject]
        protected ICertificateService CertificateService { get; set; }
        [Parameter]
        public string Identifier { get; set; }
        public User User { get; set; }
        protected List<Certificate> Certificates { get; set; } = new List<Certificate>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ValidationResult<User> user = await CertificateService.GetUserAsync(Identifier);
                User = user.Result;
                StateHasChanged();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await foreach (Certificate certificate in CertificateService.GetCertificatesAsync(Identifier))
            {
                Certificates.Add(certificate);
                this.StateHasChanged();
            }
        }
    }
}
