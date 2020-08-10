using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrameworkBaseData.Models
{
    public class Role : BaseModel
    {
        [Column(Order = 2)]
        public string Code { get; set; }

        [Column(Order = 3)]
        public string Name { get; set; }

        [Column(Order = 4)]
        public int Level { get; set; }

        //[Column(Order = 5)]
        //public virtual IList<User> Users { get; set; }
    }
}