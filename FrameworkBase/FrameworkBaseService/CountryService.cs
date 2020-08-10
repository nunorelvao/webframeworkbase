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
    public class CountryService : ICountryService
    {
        private ILogger<LocalizationService> _logger;
        private IRepository<Localization> _localizationRepository;
        private IRepository<Country> _countryRepository;
        private IRestService<Country> _countryRestService;

        public CountryService(
            ILogger<LocalizationService> logger,
            IRepository<Localization> localizationRepository,
            IRepository<Country> countryRepository,
            IRestService<Country> countryRestService)
        {
            this._logger = logger;
            this._localizationRepository = localizationRepository;
            this._countryRepository = countryRepository;
            this._countryRestService = countryRestService;
        }

        IQueryable<Country> ICountryService.GetAllCountries { get => _countryRepository.GetAll(); }

        public async Task<PaginatedList<Country>> GetAllCountriesPaginated(int? page, int pageSize)
        {
            //get all countries from DB
            return await PaginatedList<Country>.CreateAsync(_countryRepository.GetAll().AsNoTracking(), page ?? 1, pageSize);
        }

        public async Task<bool> RetrieveCountries(Uri webservice_uri)
        {
            List<Country> countrysToInsert = new List<Country>();

            try
            {
                //https://localhost:5001/api/WorldCountryDummy
                var requestUrl = _countryRestService.CreateRequestUri(webservice_uri.ToString(), "");
                IEnumerable<Country> listCountries = _countryRestService.GetAsync(requestUrl).Result;

                //first delete db
                // _countryRepository.TruncateTable("Countries");
                _countryRepository.DeleteRange(_countryRepository.GetAll());

                //save to db all
                _countryRepository.InsertRange(listCountries);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RetrieveCountries ERROR : ", ex.Message);
            }


            return await Task.FromResult<bool>(true);
        }

        public Task<bool> RetrieveCountriesOld(Uri webservice_uri)
        {
            List<Country> countrysToInsert = new List<Country>();
            try
            {
                string jsonString = GetCountriesAsync(webservice_uri).Result;

                JObject json = JObject.Parse(jsonString);

                List<CountryJsonModel> countryJsonList = new List<CountryJsonModel>();

                foreach (var item in json.SelectTokens("Results").Children())
                {
                    JProperty itemJsonComplete = (JProperty)item;

                    CountryJsonModel countryRetrieved = new CountryJsonModel
                    {
                        Country_Name = itemJsonComplete?.ToArray()[0]?["Name"]?.ToString()
                    };

                    Dictionary<CountryJsonModelCapital, string> cCapitalData = new Dictionary<CountryJsonModelCapital, string>();
                    try
                    {
                        cCapitalData[CountryJsonModelCapital.DLST] = itemJsonComplete?.ToArray()[0]?["Capital"]?["DLST"]?.ToString();
                        cCapitalData[CountryJsonModelCapital.Flg] = itemJsonComplete?.ToArray()[0]?["Capital"]?["Flg"]?.ToString();
                        cCapitalData[CountryJsonModelCapital.Name] = itemJsonComplete?.ToArray()[0]?["Capital"]?["Name"]?.ToString();

                        string geoString = string.Empty;
                        foreach (JToken geovalues in itemJsonComplete?.ToArray()[0]?["Capital"]?["GeoPt"]?.Values())
                        {
                            float.TryParse(geovalues.ToString(), result: out float geovalueout);
                            geoString += geovalueout.ToString().Replace(",", ".") + ",";
                        }
                        geoString = geoString.Remove(geoString.Length - 1);

                        cCapitalData[CountryJsonModelCapital.GeoPt] = geoString;
                        countryRetrieved.Capital = cCapitalData;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "RetrieveCountries ERROR AQUIRING COUNTRY CAPITAL DATA: " + countryRetrieved.Country_Name);
                    }

                    Dictionary<CountryJsonModelGeoRectangle, float> cGeoRectangleData = new Dictionary<CountryJsonModelGeoRectangle, float>();
                    try
                    {
                        float.TryParse(itemJsonComplete?.ToArray()[0]?["GeoRectangle"]?["West"]?.ToString(), result: out float West);
                        cGeoRectangleData[CountryJsonModelGeoRectangle.West] = West;
                        float.TryParse(itemJsonComplete?.ToArray()[0]?["GeoRectangle"]?["East"]?.ToString(), result: out float East);
                        cGeoRectangleData[CountryJsonModelGeoRectangle.East] = East;
                        float.TryParse(itemJsonComplete?.ToArray()[0]?["GeoRectangle"]?["North"]?.ToString(), result: out float North);
                        cGeoRectangleData[CountryJsonModelGeoRectangle.North] = North;
                        float.TryParse(itemJsonComplete?.ToArray()[0]?["GeoRectangle"]?["South"]?.ToString(), result: out float South);
                        cGeoRectangleData[CountryJsonModelGeoRectangle.South] = South;
                        countryRetrieved.GeoRectangle = cGeoRectangleData;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "RetrieveCountries ERROR AQUIRING COUNTRY GEORECTANGLE DATA: " + countryRetrieved.Country_Name);
                    }

                    countryRetrieved.SeqId = itemJsonComplete?.ToArray()[0]?["SeqID"]?.ToString();
                    countryRetrieved.GeoPt = new List<float>();
                    foreach (JToken geovalues in itemJsonComplete?.ToArray()[0]?["GeoPt"]?.Values())
                    {
                        float.TryParse(geovalues.ToString(), result: out float geovalueout);
                        countryRetrieved.GeoPt.Add(geovalueout);
                    }

                    countryRetrieved.TelPref = itemJsonComplete?.ToArray()[0]?["TelPref"]?.ToString();

                    Dictionary<CountryJsonModelCountryCodes, string> cCountryCodesData = new Dictionary<CountryJsonModelCountryCodes, string>();
                    try
                    {
                        cCountryCodesData[CountryJsonModelCountryCodes.tld] = itemJsonComplete?.ToArray()[0]?["CountryCodes"]?["tld"]?.ToString();
                        cCountryCodesData[CountryJsonModelCountryCodes.iso3] = itemJsonComplete?.ToArray()[0]?["CountryCodes"]?["iso3"]?.ToString();
                        cCountryCodesData[CountryJsonModelCountryCodes.iso2] = itemJsonComplete?.ToArray()[0]?["CountryCodes"]?["iso2"]?.ToString();
                        cCountryCodesData[CountryJsonModelCountryCodes.fips] = itemJsonComplete?.ToArray()[0]?["CountryCodes"]?["fips"]?.ToString();
                        cCountryCodesData[CountryJsonModelCountryCodes.isoN] = itemJsonComplete?.ToArray()[0]?["CountryCodes"]?["isoN"]?.ToString();
                        countryRetrieved.CountryCodes = cCountryCodesData;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "RetrieveCountries ERROR AQUIRING COUNTRY COUNTRYCODES DATA: " + countryRetrieved.Country_Name);
                    }

                    countryRetrieved.CountryInfoUrl = itemJsonComplete?.ToArray()[0]?["CountryInfo"]?.ToString();

                    _logger.LogDebug("RetrieveCountries Aquiring country: " + countryRetrieved.Country_Name);
                    countryJsonList.Add(countryRetrieved);
                }

                //Now save the countrys to DB
                foreach (var cJson in countryJsonList)
                {
                    Country country = new Country
                    {
                        Name = cJson?.Country_Name,
                        Code = cJson?.CountryCodes[CountryJsonModelCountryCodes.iso2].Length > 2 ? cJson?.CountryCodes[CountryJsonModelCountryCodes.iso2].Substring(0, 2) : cJson?.CountryCodes[CountryJsonModelCountryCodes.iso2],
                        Code3 = cJson?.CountryCodes[CountryJsonModelCountryCodes.iso3].Length > 3 ? cJson?.CountryCodes[CountryJsonModelCountryCodes.iso3].Substring(0, 3) : cJson?.CountryCodes[CountryJsonModelCountryCodes.iso3],
                        Extcode = cJson?.CountryCodes[CountryJsonModelCountryCodes.fips].Length > 2 ? cJson?.CountryCodes[CountryJsonModelCountryCodes.fips].Substring(0, 2) : cJson?.CountryCodes[CountryJsonModelCountryCodes.fips],
                        Domain = cJson?.TelPref?.Length > 10 ? cJson?.TelPref?.Substring(0, 10) : cJson?.TelPref
                    };
                    int.TryParse(cJson?.CountryCodes[CountryJsonModelCountryCodes.isoN], result: out int isoN);
                    country.Number = isoN;

                    countrysToInsert.Add(country);
                }

                //first delete db
                // _countryRepository.TruncateTable("Countries");
                _countryRepository.DeleteRange(_countryRepository.GetAll());

                //save to db all
                _countryRepository.InsertRange(countrysToInsert);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "ERROR RETRIEVING COUNTRIES FROM WEBSERVICE ON URI: " + webservice_uri, null);
                return Task.FromResult<bool>(false);
            }

            return Task.FromResult<bool>(true);
        }


        private async Task<string> GetCountriesAsync(Uri webservice_uri)
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
                _logger.LogError(ex, "ERROR RETRIEVING COUNTRIES FROM WEBSERVICE ON URI: " + webservice_uri, null);
            }
            return null;
        }

        public void SaveCountrys(IEnumerable<Country> countryList)
        {
            throw new NotImplementedException();
        }
    }

    public class CountryJsonModel
    {
        public string Country_Iso { get; set; }
        public string Country_Name { get; set; }
        public Dictionary<CountryJsonModelCapital, string> Capital { get; set; }
        public Array[] Capital_GeoPT { get; set; }
        public Dictionary<CountryJsonModelGeoRectangle, float> GeoRectangle { get; set; }
        public string SeqId { get; set; }
        public List<float> GeoPt { get; set; }
        public string TelPref { get; set; }
        public Dictionary<CountryJsonModelCountryCodes, string> CountryCodes { get; set; }
        public string CountryInfoUrl { get; set; }
    }

    public enum CountryJsonModelCapital
    {
        DLST,
        TD,
        Flg,
        Name,
        GeoPt
    }

    public enum CountryJsonModelGeoRectangle
    {
        West,
        East,
        North,
        South
    }

    public enum CountryJsonModelCountryCodes
    {
        tld,
        iso3,
        iso2,
        fips,
        isoN
    }
}