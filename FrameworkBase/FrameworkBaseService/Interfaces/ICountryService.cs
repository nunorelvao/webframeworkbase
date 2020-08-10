using FrameworkBaseData.Models;
using FrameworkBaseService.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBaseService.Interfaces
{
    public interface ICountryService
    {
        Task<bool> RetrieveCountries(Uri webservice_uri);
        Task<bool> RetrieveCountriesOld(Uri webservice_uri);

        IQueryable<Country> GetAllCountries { get; }

        Task<PaginatedList<Country>> GetAllCountriesPaginated(int? page, int pageSize);
    }
}