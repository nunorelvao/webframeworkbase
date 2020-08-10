using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFrameworkBase.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public int Level { get; set; }
        public string Code { get; set; }
    }
}