using FrameworkBaseData.Models;
using FrameworkBaseService.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBaseService.Interfaces
{
    public interface ILanguageService
    {
        IQueryable<Language> GetAllLanguages { get; }

        Task<PaginatedList<Language>> GetAllLanguagesPaginated(int? page, int pageSize);

        Task<Language> GetLanguageById(int personid);
    }
}