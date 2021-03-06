﻿using System.Threading.Tasks;

using RestSharp;

using UdemyPortfolio.Models.Udemy;
using UdemyPortfolio.Services.Abstracts;
using UdemyPortfolio.Services.Extensions;

namespace UdemyPortfolio.Services.Concretes
{
    public class UdemyService : IUdemyService
    {
        public async Task<Certificate> GetCertificate(string certificateCode)
        {
            RestClient client = new RestClient(@"https://www.udemy.com");
            RestRequest request = new RestRequest("api-2.0/certificates/{certificateCode}");
            request.AddParameter("fields[certificate]", "code,long_url,image_url,pdf_url,completion_date,user,course");
            request.AddParameter("fields[user]", "id,title,image_50x50,image_100x100,initials,url,url_title,name,surname,job_title");
            request.AddParameter("fields[course]", "id,title,url,image_125_H,image_240x135,image_480x270,published_title,content_info");
            request.AddUrlSegment("certificateCode", certificateCode);


            return await client.GetAsync<Certificate>(request);
        }

        public async Task<User> GetUserInfo(string certificateCode)
        {
            RestClient client = new RestClient(@"https://www.udemy.com");
            RestRequest request = new RestRequest("api-2.0/certificates/{certificateCode}");
            request.AddParameter("fields[certificate]", "user");
            request.AddParameter("fields[user]", "id,title,image_50x50,image_100x100,initials,url,url_title,name,surname,job_title");
            request.AddUrlSegment("certificateCode", certificateCode);

            Certificate certificate = await client.GetAsync<Certificate>(request);
            return certificate.User;
        }
    }
}
