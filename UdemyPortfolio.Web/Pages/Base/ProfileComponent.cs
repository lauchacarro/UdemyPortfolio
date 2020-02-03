using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using UdemyPortfolio.Models.Categories;
using UdemyPortfolio.Models.Paginator;
using UdemyPortfolio.Models.Udemy;
using UdemyPortfolio.Models.Validation;
using UdemyPortfolio.Services.Abstracts;

namespace UdemyPortfolio.Web.Pages.Base
{
    public class ProfileComponent : ComponentBase
    {
        [Inject]
        protected NavigationManager Navigation { get; set; }
        [Inject]
        protected ICertificateService CertificateService { get; set; }
        [Inject]
        protected ICategoryService CategoryService { get; set; }
        [Inject]
        protected ICertificateExcelService CertificateExcelService { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        [Parameter]
        public string Identifier { get; set; }
        public User User { get; set; }
        protected string SearchValue { get; set; }
        protected int ActualCategoryIndex { get; set; } = -1;

        private CertificatePaged _certificates = new CertificatePaged();
        protected IList<Category> Categories { get; set; } = new List<Category>();
        protected CertificatePaged CertificatePages
        {
            get { return GetCertificates(); }
            set { _certificates = value; }
        }

        private CertificatePaged GetCertificates()
        {
            IEnumerable<Certificate> filteredCertificates = _certificates.Where(x => (string.IsNullOrWhiteSpace(SearchValue) || x.Course.Title.ToLower().Contains(SearchValue.ToLower())) && (ActualCategoryIndex == -1 || Categories[ActualCategoryIndex].CertificateCodes.Contains(x.Code)));
            CertificatePaged filteredCertificatePages = (CertificatePaged)_certificates.Clone();
            filteredCertificatePages.Add(filteredCertificates);
            return filteredCertificatePages;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ValidationResult<User> user = await CertificateService.GetUserAsync(Identifier);
                User = user.Result;
                await JSRuntime.InvokeVoidAsync("setTitle", User.Title + " | Udemy Portfolio");
                this.StateHasChanged();

                await foreach (Category category in CategoryService.GetCategoriesAsync(Identifier))
                {
                    Categories.Add(category);
                }

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

        protected async Task ButtonExcel_HandleClick()
        {
            Stream stream = CertificateExcelService.ExportExcel(_certificates);
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }
            string base64 = Convert.ToBase64String(bytes);
            await JSRuntime.InvokeAsync<object>("fileSaveAs", $"{User.Title}'s Certificates.xlsx", base64);

        }

        protected void Category_HandleChanged(string categoryIndex)
        {
            ActualCategoryIndex = int.Parse(categoryIndex) - 1;
            this.StateHasChanged();
        }

        protected int Count { get { return _certificates.Count; } }
    }
}
