using System.Collections.Generic;
using System.Threading.Tasks;

using UdemyPortfolio.Models;
using UdemyPortfolio.Models.Validation;

namespace UdemyPortfolio.Services.Abstracts
{
    public interface ICertificateService
    {
        Task<ValidationResult<User>> GetUserAsync();
        Task<ValidationResult<User>> GetUserAsync(string identifier);
        Task<Validation> UploadCertificate(string certificateCode);
        IAsyncEnumerable<Certificate> GetCertificatesAsync();
        IAsyncEnumerable<Certificate> GetCertificatesAsync(string identifier);
        Task<ValidationResult<bool>> CertificateIsFromSameUser(Certificate certificate);
        Task<Validation> DeleteCertificate(string certificateCode);

    }
}
