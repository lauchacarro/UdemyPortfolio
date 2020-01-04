using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        [Parameter]
        public string Identifier { get; set; }
        public User User { get; set; }
        protected string SearchValue { get; set; }

        private List<Certificate> _certificates = new List<Certificate>();

        public List<Certificate> Certificates
        {
            get { return GetCertificates(); }
            set { _certificates = value; }
        }

        private List<Certificate> GetCertificates()
        {
            if (string.IsNullOrWhiteSpace(SearchValue))
            {
                return _certificates;
            }
            else
            {
                return _certificates.Where(x => x.Course.Title.ToLower().Contains(SearchValue.ToLower())).ToList();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ValidationResult<User> user = await CertificateService.GetUserAsync(Identifier);
                User = user.Result;
                await JSRuntime.InvokeVoidAsync("setTitle", User.Title + " | Udemy Portfolio");
                this.StateHasChanged();

                await foreach (Certificate certificate in CertificateService.GetCertificatesAsync(Identifier))
                {
                    _certificates.Add(certificate);
                    this.StateHasChanged();
                }
            }
        }

        protected void Search_HandleChange(string value)
        {
            SearchValue = value;
            this.StateHasChanged();
        }
    }
}
