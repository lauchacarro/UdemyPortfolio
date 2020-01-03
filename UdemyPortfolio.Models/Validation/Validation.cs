using System.Collections.Generic;

namespace UdemyPortfolio.Models.Validation
{
    public class Validation
    {
        public Validation()
        {
            ErrorMessages = new List<string>();
            Success = true;
        }

        public bool Success { get; set; }
        public ICollection<string> ErrorMessages { get; set; }
    }
}
