using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using UdemyPortfolio.Models.Validation;
using UdemyPortfolio.Services.Abstracts;

namespace UdemyPortfolio.Web.Components.Admin
{
    public class UploadCertificateComponent : ComponentBase
    {
        [Inject]
        protected ICertificateService CertificateService { get; set; }

        [Parameter]
        public Action OnUploaded { get; set; }

        protected string UrlCertificate { get; set; }
        protected string ErrorMessage { get; set; }
        protected bool InvalidUrl { get; set; }

        bool IsCertificateUrlValid(out string[] paths)
        {
            paths = null;
            if (Uri.TryCreate(UrlCertificate, UriKind.Absolute, out Uri uri))
            {
                paths = uri.OriginalString.Split("/").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                if (uri.Scheme == "https" && uri.Host == "www.udemy.com" && paths[2] == "certificate")
                {
                    return true;
                }
            }
            ErrorMessage = "Link del certificado invalido";
            return false;
        }

        protected async Task Button_OnClick()
        {
            InvalidUrl = false;

            if (IsCertificateUrlValid(out string[] paths))
            {
                string certificateCode = paths[3];

                Validation result = await CertificateService.UploadCertificate(certificateCode);
                if (!result.Success)
                {
                    InvalidUrl = true;
                    ErrorMessage = result.ErrorMessages.First();
                }
                if (!InvalidUrl)
                {
                    OnUploaded();
                }
            }
            else
            {
                InvalidUrl = true;
            }

        }
    }
}
