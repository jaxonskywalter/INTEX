using System;
namespace INTEX.Models.ViewModels
{
    public class PageInfo
    {
        // information needed for pages
        public int TotalNumRecords { get; set; }
        public int RecordsPerPage { get; set; }
        public int CurrentPage { get; set; }

        // Calculate how many pages we need
        public int TotalPages => (int)Math.Ceiling((double)TotalNumRecords / RecordsPerPage);
    }

}
