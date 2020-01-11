using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UdemyPortfolio.Models.Paginator;
using UdemyPortfolio.Models.Udemy;

namespace UdemyPortfolio.Services.Abstracts
{
    public interface ICertificateExcelService
    {
        Stream ExportExcel(IEnumerable<Certificate> certificates);
    }
}
