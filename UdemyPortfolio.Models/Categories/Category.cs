using System.Collections.Generic;

namespace UdemyPortfolio.Models.Categories
{
    public class Category
    {
        public Category()
        {
            CertificateCodes = new List<string>();
        }

        public string Name { get; set; }
        public IEnumerable<string> CertificateCodes { get; set; }
    }
}
