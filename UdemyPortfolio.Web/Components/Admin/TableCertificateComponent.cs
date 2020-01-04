using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using UdemyPortfolio.Models;
using UdemyPortfolio.Services.Abstracts;

namespace UdemyPortfolio.Web.Components.Admin
{
    public class TableCertificateComponent : ComponentBase
    {
        [Inject]
        protected ICertificateService CertificateService { get; set; }
        protected List<Certificate> Certificates { get; set; } = new List<Certificate>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await foreach (Certificate certificate in CertificateService.GetCertificatesAsync())
                {
                    Certificates.Add(certificate);
                    this.StateHasChanged();
                }
            }
        }
    }
}
