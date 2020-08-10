using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFrameworkBase.Areas.BO.Models
{
    public class CountryListViewModel
    {
        public CountryListViewModel()
        {
            CountryList = new List<CountryViewModel>();
        }

        public List<CountryViewModel> CountryList { get; set; }

        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}