namespace UdemyPortfolio.Services.Abstracts
{
    public interface IPathService
    {
        string GetUserFolder();
        string GetUserFolder(string identifier);
        string GetCertificateFolder();
        string GetCertificateFolder(string identifier);
        string GetUserIdentifier();
    }
}
