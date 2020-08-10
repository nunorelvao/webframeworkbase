using FrameworkBaseData.Models;
using FrameworkBaseService.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBaseService.Interfaces
{
    public interface ICurrencyService
    {
        Task<bool> RetrieveCurrencies(Uri webservice_uri);

        IQueryable<Currency> GetAllCurrencies { get; }

        Task<PaginatedList<Currency>> GetAllCurrenciesPaginated(int? page, int pageSize);
    }
}