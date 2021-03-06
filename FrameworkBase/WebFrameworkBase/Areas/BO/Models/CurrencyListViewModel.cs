﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFrameworkBase.Areas.BO.Models
{
    public class CurrencyListViewModel
    {
        public CurrencyListViewModel()
        {
            CurrencyList = new List<CurrencyViewModel>();
        }

        public List<CurrencyViewModel> CurrencyList { get; set; }

        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}