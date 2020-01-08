using System;

namespace UdemyPortfolio.Models.Paginator
{
    public class PagedResultBase
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; protected set; } = 5;
        public int RowCount { get; set; }

        public int PageCount
        {

            get { return Convert.ToInt32(Math.Ceiling((double)RowCount / PageSize)); }
        }

        public int FirstRowOnPage
        {

            get { return PageSize * (CurrentPage - 1); }
        }

        public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }

        public int PreviewPage
        {
            get { return Math.Max(CurrentPage - 1, 1); }
        }

        public int NextPage
        {
            get { return Math.Min(CurrentPage + 1, PageCount); }
        }
    }
}
