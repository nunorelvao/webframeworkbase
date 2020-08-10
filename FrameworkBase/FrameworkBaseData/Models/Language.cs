using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrameworkBaseData.Models
{
    public class Language : BaseModel
    {
        [Column(Order = 2)]
        public string Code { get; set; }

        [Column(Order = 3)]
        public string Name { get; set; }

        [InverseProperty("Language")]
        public virtual IList<Country> Countries { get; set; }
    }
}