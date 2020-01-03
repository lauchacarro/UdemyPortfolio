using UdemyPortfolio.Models;

namespace UdemyPortfolio.Services.Abstracts
{
    public interface IUdemyService
    {
        User GetUserInfo(string certificateCode);
        Certificate GetCertificate(string certificateCode);
    }
}
