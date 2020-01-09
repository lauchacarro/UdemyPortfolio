using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

using UdemyPortfolio.Models;
using UdemyPortfolio.Models.GitHub;
using UdemyPortfolio.Models.Validation;
using UdemyPortfolio.Services.Abstracts;

namespace UdemyPortfolio.Services.Concretes
{
    public class CertificateService : ICertificateService
    {
        private readonly IGitHubService _githubService;
        private readonly IUdemyService _udemyService;
        private readonly IPathService _pathService;

        public CertificateService(IGitHubService githubService, IUdemyService udemyService, IPathService pathService)
        {
            _githubService = githubService ?? throw new ArgumentNullException(nameof(githubService));
            _udemyService = udemyService ?? throw new ArgumentNullException(nameof(udemyService));
            _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
        }

        public async Task<ValidationResult<bool>> CertificateIsFromSameUser(Certificate certificate)
        {
            ValidationResult<bool> validationResult = new ValidationResult<bool>();
            string userPath = _pathService.GetUserFolder();
            FileContent firstCertificateFile = await _githubService.GetFileContentAsync(userPath, "FirstCertificate");

            Certificate firstCertificate = _udemyService.GetCertificate(firstCertificateFile.Content);
            validationResult.Result = firstCertificate.User.Url_Title.Equals(certificate.User.Url_Title);
            return validationResult;
        }

        public async Task<Validation> DeleteCertificate(string certificateCode)
        {
            Validation validation = new Validation();
            string certificatePath = _pathService.GetCertificateFolder();
            await _githubService.DeleteFileAsync(certificatePath, certificateCode);
            return validation;
        }

        public async IAsyncEnumerable<Certificate> GetCertificatesAsync()
        {
            string certificatePath = _pathService.GetCertificateFolder();
            IEnumerable<FileContent> certificateCodes = await _githubService.GetAllFilesAsync(certificatePath) ?? new List<FileContent>();

            foreach (FileContent file in certificateCodes)
            {
                Certificate certificate = _udemyService.GetCertificate(file.Name);
                yield return certificate;
            }
        }

        public async IAsyncEnumerable<Certificate> GetCertificatesAsync(string identifier)
        {
            string certificatePath = _pathService.GetCertificateFolder(identifier);
            IEnumerable<FileContent> certificateCodes = await _githubService.GetAllFilesAsync(certificatePath) ?? new List<FileContent>();

            foreach (FileContent file in certificateCodes)
            {
                Certificate certificate = _udemyService.GetCertificate(file.Name);
                yield return certificate;
            }
        }

        public async Task<ValidationResult<User>> GetUserAsync()
        {
            ValidationResult<User> validationResult = new ValidationResult<User>();

            string userPath = _pathService.GetUserFolder();
            FileContent firstCertificate = await _githubService.GetFileContentAsync(userPath, "FirstCertificate");

            if (firstCertificate is null)
            {
                validationResult.Success = false;
                validationResult.ErrorMessages.Add("No hay información del usuario debido a que nunca subió un certificado.");
            }
            else
            {
                validationResult.Result = _udemyService.GetUserInfo(firstCertificate.Content);
            }

            return validationResult;
        }

        public async Task<ValidationResult<User>> GetUserAsync(string identifier)
        {
            ValidationResult<User> validationResult = new ValidationResult<User>();

            string userPath = _pathService.GetUserFolder(identifier);
            FileContent firstCertificate = await _githubService.GetFileContentAsync(userPath, "FirstCertificate");

            if (firstCertificate is null)
            {
                validationResult.Success = false;
                validationResult.ErrorMessages.Add("No hay información del usuario debido a que nunca subió un certificado.");
            }
            else
            {
                validationResult.Result = _udemyService.GetUserInfo(firstCertificate.Content);
            }

            return validationResult;
        }

        public async Task<Validation> UploadCertificate(string certificateCode)
        {
            Validation validation = new Validation();

            string userPath = _pathService.GetUserFolder();
            string certificatePath = _pathService.GetCertificateFolder();

            bool hasCertificate = await _githubService.ExistFileAsync(certificatePath, certificateCode);

            if (hasCertificate)
            {
                validation.ErrorMessages.Add("Este certificado ya fue agregado. Pruebe con otro.");
                validation.Success = false;
            }
            else
            {
                Certificate certificate = _udemyService.GetCertificate(certificateCode);

                if (certificate.Code is null)
                {
                    validation.ErrorMessages.Add("No se encontro el certificado. Revise el link ingresado.");
                    validation.Success = false;
                }
                else
                {
                    Validation hasUserInfo = await GetUserAsync();
                    if (!hasUserInfo.Success)
                    {
                        await _githubService.CreateFileAsync(userPath, "FirstCertificate", certificateCode);
                    }

                    ValidationResult<bool> isFromSameUser = await CertificateIsFromSameUser(certificate);
                    if (isFromSameUser.Result)
                    {
                        string certificateInfo = JsonSerializer.Serialize(certificate);
                        await _githubService.CreateFileAsync(certificatePath, certificateCode, certificateInfo);
                    }
                    else
                    {
                        validation.ErrorMessages.Add("El certificado le pertenece a otro usuario.");
                        validation.Success = false;
                    }
                }
            }

            return validation;
        }
    }
}
