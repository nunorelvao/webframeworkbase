using FrameworkBaseData.Models;
using FrameworkBaseService.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBaseService.Domain.Interfaces
{
    internal interface IPersonDomain
    {
        Task<PersonDomainValidation> Validate(Person person);
    }
}