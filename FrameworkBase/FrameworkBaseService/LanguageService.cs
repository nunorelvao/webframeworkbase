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
    public class LanguageService : ILanguageService
    {
        private ILogger<LocalizationService> _logger;
        private IRepository<Localization> _localizationRepository;
        private IRepository<Language> _languageRepository;

        public LanguageService(
            ILogger<LocalizationService> logger,
            IRepository<Localization> localizationRepository,
            IRepository<Language> languageRepository)
        {
            this._logger = logger;
            this._localizationRepository = localizationRepository;
            this._languageRepository = languageRepository;
        }

        IQueryable<Language> ILanguageService.GetAllLanguages { get => _languageRepository.GetAll(l => l.Countries).AsQueryable(); }

        public async Task<PaginatedList<Language>> GetAllLanguagesPaginated(int? page, int pageSize)
        {
            //get all countries from DB
            return await PaginatedList<Language>.Create(_languageRepository.GetAll(e => e.Countries), page ?? 1, pageSize);
        }

        public async Task<Language> GetLanguageById(int languageid)
        {
            return await Task.FromResult(_languageRepository.Get(languageid, p => p.Countries));
        }
    }
}