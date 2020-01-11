using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ClosedXML.Excel;
using UdemyPortfolio.Models.Udemy;
using UdemyPortfolio.Services.Abstracts;

namespace UdemyPortfolio.Services.Concretes
{
    public class CertificateExcelService : ICertificateExcelService
    {
        public Stream ExportExcel(IEnumerable<Certificate> certificates)
        {
            string[] headers = new string[]
            {
                "Code",
                "Course",
                "Content Info",
                "Completion Date",
                "Course Url",
                "Certificate Url"
            };

            var data = certificates.Select(cer => new
            {
                cer.Code,
                cer.Course.Title,
                cer.Course.Content_info,
                cer.Completion_date,
                cer.Course.Url,
                cer.Long_url
            });

            XLWorkbook wb = new XLWorkbook();
            IXLWorksheet ws = wb.Worksheets.Add("Certificates");

            ws.Cell(2, 1).InsertData(data);
            for (int i = 0; i < headers.Length; i++)
            {
                IXLCell cell = ws.Cell(1, i + 1);
                cell.Value = headers[i];
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.FromArgb(63, 63, 118);
                cell.Style.Fill.BackgroundColor = XLColor.FromArgb(255, 204, 153);
                ws.Column(i + 1).AdjustToContents();
            }
            Stream fs = new MemoryStream();
            wb.SaveAs(fs);
            fs.Position = 0;

            return fs;
        }
    }
}
