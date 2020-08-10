using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkBaseService.Domain.Errors
{
    public class PersonDomainValidation
    {
        public Enum INSERTVALIDATIONS { get; set; }
    }

    public enum INSERTVALIDATIONS
    {
        FIRSTNAMELASTANAMEALREADYEXISTS,
        UNKNOWNERROR,
        SUCESS
    }
}