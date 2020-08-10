using FrameworkBaseData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFrameworkBase.Areas.BO.Models
{
    public class PersonIndexViewModel
    {
        public IEnumerable<Person> PersonsList { get; set; }
    }
}