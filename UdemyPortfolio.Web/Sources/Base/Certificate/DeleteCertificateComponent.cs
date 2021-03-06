﻿using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using UdemyPortfolio.Services.Abstracts;

namespace UdemyPortfolio.Web.Sources.Base
{
    public class DeleteCertificateComponent : ComponentBase
    {
        [Inject]
        protected ICertificateService CertificateService { get; set; }

        [Parameter]
        public string CertificateCode { get; set; }
        [Parameter]
        public EventCallback<string> OnDeleted { get; set; }

        protected async Task Button_OnClick()
        {
            await CertificateService.DeleteCertificate(CertificateCode);
            await OnDeleted.InvokeAsync(CertificateCode);
        }
    }
}
