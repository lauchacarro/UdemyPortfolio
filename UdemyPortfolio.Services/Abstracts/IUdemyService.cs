using System.Threading.Tasks;

using UdemyPortfolio.Models.Udemy;

namespace UdemyPortfolio.Services.Abstracts
{
    public interface IUdemyService
    {
        Task<User> GetUserInfo(string certificateCode);
        Task<Certificate> GetCertificate(string certificateCode);
    }
}
