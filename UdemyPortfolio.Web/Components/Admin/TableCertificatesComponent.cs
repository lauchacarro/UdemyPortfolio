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
        protected PagedResult<Certificate> CertificatePages { get; set; } = new PagedResult<Certificate>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await foreach (Certificate certificate in CertificateService.GetCertificatesAsync())
                {
                    CertificatePages.Results.Add(certificate);
                    CertificatePages.RowCount = CertificatePages.Results.Count;
                    this.StateHasChanged();
                }
            }
        }

        protected void HandlePageChanged(int page)
        {
            CertificatePages.CurrentPage = page;
            this.StateHasChanged();
        }
    }
}
