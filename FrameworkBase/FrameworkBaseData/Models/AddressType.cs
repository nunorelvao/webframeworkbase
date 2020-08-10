using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrameworkBaseData.Models
{
    public class AddressType : BaseModel
    {
        [Column(Order = 2)]
        public string Code { get; set; }

        [Column(Order = 3)]
        public string Name { get; set; }
    }
}