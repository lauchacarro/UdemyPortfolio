using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using UdemyPortfolio.Models;
using UdemyPortfolio.Models.Paginator;
using UdemyPortfolio.Services.Abstracts;

namespace UdemyPortfolio.Web.Components.Admin
{
    public class TableCertificatesComponent : ComponentBase
    {
        [Inject]
        protected ICertificateService CertificateService { get; set; }
        protected CertificatePaged CertificatePages { get; set; } = new CertificatePaged();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await foreach (Certificate certificate in CertificateService.GetCertificatesAsync())
                {
                    CertificatePages.Add(certificate);
                    this.StateHasChanged();
                }
            }
        }

        protected void Paginator_HandleChanged(int page)
        {
            CertificatePages.CurrentPage = page;
            this.StateHasChanged();
        }
    }
}
