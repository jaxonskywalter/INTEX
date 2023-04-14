using System;
using System.Linq;

namespace INTEX.Models.ViewModels
{
    
    public class RecordsViewModel
    {

        // IQueryable is a type of variable that allows us to pass the list of books with all of their information and order it
        public IQueryable<Burialmain> Records { get; set; }

        public IQueryable<Bodyanalysischartreal> BARecords { get; set; }

        public PageInfo PageInfo { get; set; }

        internal object Where(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }
    
}
