using FrameworkBaseData.Models;
using FrameworkBaseRepo;
using FrameworkBaseService.Interfaces;
using FrameworkBaseService.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBaseService
{
    public class CurrencyService : ICurrencyService
    {
        private ILogger<LocalizationService> _logger;
        private IRepository<Localization> _localizationRepository;
        private IRepository<Currency> _currencyRepository;

        public CurrencyService(
            ILogger<LocalizationService> logger,
            IRepository<Localization> localizationRepository,
            IRepository<Currency> currencyRepository)
        {
            this._logger = logger;
            this._localizationRepository = localizationRepository;
            this._currencyRepository = currencyRepository;
        }

        IQueryable<Currency> ICurrencyService.GetAllCurrencies { get => _currencyRepository.GetAll(); }

        public async Task<PaginatedList<Currency>> GetAllCurrenciesPaginated(int? page, int pageSize)
        {
            //get all countries from DB
            return await PaginatedList<Currency>.CreateAsync(_currencyRepository.GetAll().AsNoTracking(), page ?? 1, pageSize);
        }

        Task<bool> ICurrencyService.RetrieveCurrencies(Uri webservice_uri)
        {
            List<Currency> currenciesToInsert = new List<Currency>();
            try
            {
                string jsonString = GetCurrenciesAsync(webservice_uri).Result;

                JObject json = JObject.Parse(jsonString);

                List<CurrencyJsonModel> currencyJsonList = new List<CurrencyJsonModel>();

                foreach (var item in json.Children())
                {
                    JProperty itemJsonComplete = (JProperty)item;

                    int.TryParse(itemJsonComplete?.ToArray()[0]?["decimal_digits"]?.ToString(), out int decimaldigits);

                    decimal.TryParse(itemJsonComplete?.ToArray()[0]?["rounding"]?.ToString(), out decimal rounding);

                    CurrencyJsonModel currencyRetrieved = new CurrencyJsonModel
                    {
                        Code = itemJsonComplete?.ToArray()[0]?["code"]?.ToString(),
                        Name = itemJsonComplete?.ToArray()[0]?["code"]?.ToString(),
                        Symbol = itemJsonComplete?.ToArray()[0]?["symbol"]?.ToString(),
                        Symbol_Native = itemJsonComplete?.ToArray()[0]?["symbol_native"]?.ToString(),
                        Decimal_Digits = decimaldigits,
                        Rounding = rounding
                    };

                    _logger.LogDebug("RetrieveCurrencies Aquiring currency: " + currencyRetrieved.Code);
                    currencyJsonList.Add(currencyRetrieved);
                }

                //Now save the countrys to DB
                foreach (var cJson in currencyJsonList)
                {
                    Currency curency = new Currency
                    {
                        Code = cJson?.Code,
                        Name = cJson?.Name,
                        Symbol = cJson?.Symbol,
                        SymbolNative = cJson?.Symbol_Native,
                        DecimalDigits = cJson.Decimal_Digits,
                        Rounding = cJson.Rounding
                    };

                    //LOG ERORS
                    if (curency.Code.Length > 3)
                    {
                        _logger.LogError("ERROR RETRIEVING CURRENCY CODE > 3: " + curency.Code);
                    }
                    else if (curency.Symbol.Length > 3)
                    {
                        _logger.LogError("ERROR RETRIEVING CURRENCY SYMBOL > 3: " + curency.Symbol);
                    }
                    else if (curency.SymbolNative.Length > 10)
                    {
                        _logger.LogError("ERROR RETRIEVING CURRENCY SymbolNative > 3: " + curency.SymbolNative);
                    }
                    else
                    {
                        currenciesToInsert.Add(curency);
                    }
                }

                //first delete db
                _currencyRepository.DeleteRange(_currencyRepository.GetAll());

                //save to db all
                _currencyRepository.InsertRange(currenciesToInsert);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "ERROR RETRIEVING CURRENCIES FROM WEBSERVICE ON URI: " + webservice_uri, null);
                return Task.FromResult<bool>(false);
            }

            return Task.FromResult<bool>(true);
        }

        private async Task<string> GetCurrenciesAsync(Uri webservice_uri)
        {
            try
            {
                HttpClient client = new HttpClient();
                string returnResult = string.Empty;
                HttpResponseMessage response = await client.GetAsync(webservice_uri);
                if (response.IsSuccessStatusCode)
                {
                    returnResult = await response.Content.ReadAsStringAsync();
                }
                return returnResult;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "ERROR RETRIEVING CURRENCIES FROM WEBSERVICE ON URI: " + webservice_uri, null);
            }
            return null;
        }
    }

    public class CurrencyJsonModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Symbol_Native { get; set; }
        public int Decimal_Digits { get; set; }
        public decimal Rounding { get; set; }
    }
}