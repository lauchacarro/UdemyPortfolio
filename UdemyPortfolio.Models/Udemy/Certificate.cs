using System;

namespace UdemyPortfolio.Models
{
    public class Certificate
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime Completion_date { get; set; }
        public string Image_url { get; set; }
        public User User { get; set; }
        public Course Course { get; set; }
        public string Pdf_url { get; set; }
        public string Long_url { get; set; }

    }
}
