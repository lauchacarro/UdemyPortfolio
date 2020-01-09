using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using UdemyPortfolio.Models.Paginator;
using UdemyPortfolio.Models.Udemy;
using UdemyPortfolio.Services.Abstracts;

namespace UdemyPortfolio.Web.Sources.Base
{
    public class TableCertificatesComponent : ComponentBase
    {
        [Inject]
        protected ICertificateService CertificateService { get; set; }
        [Inject]
        protected NavigationManager Navigation { get; set; }

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

        protected void Certificate_HandleDeleted(string certificateCode)
        {
            List<Certificate> certificatesFiltered = new List<Certificate>();

            IEnumerable<Certificate> enumerableCertificatesFiltered = CertificatePages.Where(x => x.Code != certificateCode);
            certificatesFiltered.AddRange(enumerableCertificatesFiltered);

            CertificatePages.Clear();
            CertificatePages.Add(certificatesFiltered);
            this.StateHasChanged();
        }

        protected int Count { get { return CertificatePages.Count; } }
    }
}
