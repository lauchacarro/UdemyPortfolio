using System.Collections.Generic;

namespace UdemyPortfolio.Models.Paginator
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; }

        public PagedResult()
        {
            CurrentPage = 1;
            Results = new List<T>();
        }
    }
}
