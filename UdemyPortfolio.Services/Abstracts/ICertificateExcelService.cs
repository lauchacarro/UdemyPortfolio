using System.Collections.Generic;
using System.IO;

using UdemyPortfolio.Models.Udemy;

namespace UdemyPortfolio.Services.Abstracts
{
    public interface ICertificateExcelService
    {
        Stream ExportExcel(IEnumerable<Certificate> certificates);
    }
}
