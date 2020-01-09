using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using UdemyPortfolio.Models;
using UdemyPortfolio.Models.Paginator;
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

        private CertificatePaged _certificates = new CertificatePaged();
        protected CertificatePaged CertificatePages
        {
            get { return GetCertificates(); }
            set { _certificates = value; }
        }

        private CertificatePaged GetCertificates()
        {
            if (string.IsNullOrWhiteSpace(SearchValue))
            {
                return _certificates;
            }
            else
            {
                IEnumerable<Certificate> filteredCertificates = _certificates.Where(x => x.Course.Title.ToLower().Contains(SearchValue.ToLower()));
                CertificatePaged filteredCertificatePages = (CertificatePaged)_certificates.Clone();
                filteredCertificatePages.Add(filteredCertificates);
                return filteredCertificatePages;
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
            _certificates.CurrentPage = 1;
            this.StateHasChanged();
        }

        protected void Paginator_HandleChanged(int page)
        {
            _certificates.CurrentPage = page;
            this.StateHasChanged();
        }

        protected int Count { get { return _certificates.Count; } }
    }
}
