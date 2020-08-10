using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serilog;
using Microsoft.Extensions.Logging;
using FrameworkBaseData;
using FrameworkBaseRepo;
using FrameworkBaseData.Models;
using FrameworkBaseService.Interfaces;

namespace FrameworkBaseService
{
    public class LocalizationService : ILocalizationService
    {
        private ILogger<LocalizationService> _logger;
        private IRepository<Localization> _localizationRepository;
        private IRepository<Language> _languageRepository;

        public LocalizationService(
            ILogger<LocalizationService> logger,
            IRepository<Localization> localizationRepository,
            IRepository<Language> languageRepository)
        {
            this._logger = logger;
            this._localizationRepository = localizationRepository;
            this._languageRepository = languageRepository;
        }

        public string GetLocalizedValue(string key, string languagecode)
        {
            _logger.LogDebug("LocalizationService.GetLocalizedValue START - Input: {key} - Language:  {languagecode}", key, languagecode);

            string resultLocalized = key;

            try
            {
                key = !string.IsNullOrEmpty(key) ? key.Trim() : string.Empty;
                languagecode = !string.IsNullOrEmpty(languagecode) ? languagecode.Trim() : string.Empty;

                ////GET DUMMY
                //var localizedDataDummy = dummyLocalization.SingleOrDefault(lv =>
                //lv.key.Equals(key, StringComparison.CurrentCultureIgnoreCase) &&
                //lv.language_code.Equals(languagecode, StringComparison.CurrentCultureIgnoreCase));
                //if (localizedDataDummy != null)
                //{
                //    return localizedDataDummy.value;
                //}

                //GET FROM REPO
                var theLanguage = _languageRepository.GetAll().FirstOrDefault(l => l.Code.Equals(languagecode, StringComparison.CurrentCultureIgnoreCase));
                int theLanguageID = theLanguage != null ? theLanguage.Id : 0;

                var localizedData = _localizationRepository.GetAll().SingleOrDefault(lv =>
                                        lv.Localizationkey.Equals(key, StringComparison.CurrentCultureIgnoreCase) &&
                                        (theLanguage != null && lv.Language == theLanguage));

                if (localizedData != null)
                {
                    resultLocalized = localizedData.Localizationvalue;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"LocalizationService.GetLocalizedValue ERROR: {ex}");
                throw ex;
            }

            _logger.LogDebug("LocalizationService.GetLocalizedValue END - Return: {resultLocalized}", resultLocalized);

            return resultLocalized;
        }

        public Dictionary<string, string> GetLocalizedValues(string[] keys, string languagecode)
        {
            Dictionary<string, string> resultsLocalized = new Dictionary<string, string>();

            try
            {
                //_logger.LogDebug("LocalizationService.GetLocalizedValues START - Input: {key} - Language:  {languagecode}", key, languagecode);

                languagecode = !string.IsNullOrEmpty(languagecode) ? languagecode.Trim() : string.Empty;

                //GET FROM REPO
                var theLanguage = _languageRepository.GetAll().FirstOrDefault(l => l.Code.Equals(languagecode, StringComparison.CurrentCultureIgnoreCase));
                int theLanguageID = theLanguage != null ? theLanguage.Id : 0;

                var localizedData = _localizationRepository.GetAll().Where(lv => keys.Contains(lv.Localizationkey) &&
                                        (theLanguage != null && lv.Language == theLanguage)).ToDictionary(d => d.Localizationkey, d => d.Localizationvalue);

                if (localizedData != null)
                {
                    resultsLocalized = localizedData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"LocalizationService.GetLocalizedValue ERROR: {ex}");
                throw ex;
            }

            //_logger.LogDebug("LocalizationService.GetLocalizedValue END - Return: {resultLocalized}", resultLocalized);

            return resultsLocalized;
        }

        public bool SetLocalizedValue(string key, string value, string languagecode)
        {
            try
            {
                var theLanguage = _languageRepository.GetAll().FirstOrDefault(l => l.Code.Equals(languagecode, StringComparison.CurrentCultureIgnoreCase));

                if (theLanguage == null)
                {
                    _logger.LogError($"LocalizationService.SetLocalizedValue LanguageCode: {languagecode}, does not exit in DB!");
                    return false;
                }

                var localizedData = _localizationRepository.GetAll().SingleOrDefault(lv =>
                                                   lv.Localizationkey.Equals(key, StringComparison.CurrentCultureIgnoreCase) &&
                                                    (theLanguage != null && lv.Language == theLanguage));

                //TODO: Implement common entity models properties to be updated
                Localization loc = new Localization();
                //if exists update
                if (localizedData != null)
                {
                    loc = localizedData;
                    loc.Localizationvalue = value;

                    _localizationRepository.Update(loc);
                }
                else //insert new
                {
                    loc.Localizationkey = key;
                    loc.Localizationvalue = value;

                    _localizationRepository.Insert(loc);
                }

                return true;
            }
            catch (Exception exp)
            {
                _logger.LogCritical("LocalizationService.SetLocalizedValue {exp}", exp);
                return false;
            }
        }

        #region Dummy Data

        public List<DummyLocalizeData> DummyLocalization = new List<DummyLocalizeData>()
        {
            new DummyLocalizeData(){Id = 0, Key = "hello", Value = "olá", Language_Code = "pt"},
            new DummyLocalizeData(){Id = 1, Key = "hello", Value = "alo", Language_Code = "fr"},
            new DummyLocalizeData(){Id = 2, Key = "hello", Value = "hello", Language_Code = "en"},
            new DummyLocalizeData(){Id = 3, Key = "world", Value = "mundo", Language_Code = "pt"},
            new DummyLocalizeData(){Id = 4, Key = "world", Value = "monde", Language_Code = "fr"},
            new DummyLocalizeData(){Id = 5, Key = "world", Value = "world", Language_Code = "en"},
        };

        public class DummyLocalizeData
        {
            public int Id { get; set; }
            public string Key { get; set; }
            public string Value { get; set; }
            public string Language_Code { get; set; }
        }

        #endregion Dummy Data
    }
}