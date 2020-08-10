using FrameworkBaseData.Models;
using FrameworkBaseRepo;
using FrameworkBaseService.Interfaces;
using FrameworkBaseService.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBaseService
{
    public class PersonService : IPersonService
    {
        private ILogger<LocalizationService> _logger;
        private IRepository<Localization> _localizationRepository;
        private IRepository<Person> _personRepository;

        public PersonService(
            ILogger<LocalizationService> logger,
            IRepository<Localization> localizationRepository,
            IRepository<Person> personRepository)
        {
            this._logger = logger;
            this._localizationRepository = localizationRepository;
            this._personRepository = personRepository;
        }

        IQueryable<Person> IPersonService.GetAllPersons { get => _personRepository.GetAll(); }

        public async Task<Person> AddNewPerson(string firstName, string middleName, string lastName, DateTime? bornDate, int languageId)
        {
            Person person = new Person();

            person.Firstname = firstName;
            person.Middlename = middleName;
            person.Lastname = lastName;
            person.Borndate = bornDate;
            person.Languageid = languageId;

            return await Task.FromResult(_personRepository.Insert(person));
        }

        public async Task<PaginatedList<Person>> GetAllPersonsPaginated(int? page, int pageSize)
        {
            return await PaginatedList<Person>.Create(_personRepository.GetAll(e => e.Language, e => e.Personaddresses, e => e.Persondocuments, p => p.User), page ?? 1, pageSize);
        }

        public async Task<Person> GetPersonById(int personId)
        {
            return await Task.FromResult(_personRepository.Get(personId, p => p.Language, p => p.User));
        }

        public async Task<string> GetPersonFullName(Person person)
        {
            return await Task.FromResult(person.Firstname +
                (string.IsNullOrWhiteSpace(person.Middlename) ? "" : " " + person.Middlename) +
                (string.IsNullOrWhiteSpace(person.Lastname) ? "" : " " + person.Lastname));
        }

        public async Task<bool> UdpatePerson(int personId, string firstName, string middleName, string lastName, DateTime? bornDate, int languageId)
        {
            try
            {
                Person person = _personRepository.Get(personId);

                if (person != null)
                {
                    person.Firstname = firstName;
                    person.Middlename = middleName;
                    person.Lastname = lastName;
                    person.Borndate = bornDate;
                    person.Languageid = languageId;

                    _personRepository.Update(person);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }
    }
}