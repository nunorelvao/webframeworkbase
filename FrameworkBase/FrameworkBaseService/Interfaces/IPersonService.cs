using FrameworkBaseData.Models;
using FrameworkBaseService.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBaseService.Interfaces
{
    public interface IPersonService
    {
        IQueryable<Person> GetAllPersons { get; }

        Task<PaginatedList<Person>> GetAllPersonsPaginated(int? page, int pageSize);

        Task<Person> GetPersonById(int personId);

        Task<string> GetPersonFullName(Person person);

        Task<bool> UdpatePerson(int personId, string firstName, string middleName, string lastName, DateTime? bornDate, int languageId);

        Task<Person> AddNewPerson(string firstName, string middleName, string lastName, DateTime? bornDate, int languageId);
    }
}