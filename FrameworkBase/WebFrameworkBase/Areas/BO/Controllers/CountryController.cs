using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebFrameworkBase.Areas.BO.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using FrameworkBaseService.Interfaces;
using FrameworkBaseData.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebFrameworkBase.Models;

namespace WebFrameworkBase.Areas.BO.Controllers
{
    [Area("BO")]
    [Authorize]
    public class CountryController : BaseController
    {
        private readonly ILocalizationService _localizationservice;
        private ILogger<CountryController> _logger;
        private ICountryService _countryservice;

        public CountryController(
             ILocalizationService localizationservice,
           ICountryService countryservice,
           ILogger<CountryController> logger,
           UserManager<ApplicationUser> usermanager) : base(usermanager)
        {
            _localizationservice = localizationservice;
            _countryservice = countryservice;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(GetCountryIndexModel(null).Result);
        }

        [HttpPost]
        public IActionResult Index(int? page)
        {
            bool isAjaxCall = HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";

            ViewBag.RetrieveCountriesTooltip = _localizationservice.GetLocalizedValue("COUNTRIESTOOLTIP", GetApplicationUserLanguageCode());
            return PartialView(GetCountryIndexModel(page).Result);
        }

        private async Task<CountryListViewModel> GetCountryIndexModel(int? page, int pagesize = 10)
        {
            CountryListViewModel model = new CountryListViewModel
            {
                PageSize = pagesize
            };
            var modelPaginated = _countryservice.GetAllCountriesPaginated(page, model.PageSize).Result;

            modelPaginated.ForEach(c => model.CountryList.Add(new CountryViewModel()
            {
                CountryId = c.Id,
                CountryName = c.Name,
                CountryIsoCode = c.Code,
                CountryIsoCode3 = c.Code3
            }));

            model.HasPreviousPage = modelPaginated.HasPreviousPage;
            model.HasNextPage = modelPaginated.HasNextPage;
            model.TotalPages = modelPaginated.TotalPages;
            model.PageIndex = page ?? 1;
            return await Task.FromResult(model);
        }

        [HttpGet]
        public IActionResult RetrieveCountries()
        {


            CountryRetrieveViewModel model = new CountryRetrieveViewModel() { ServiceURL = "https://localhost:5001/api/WorldCountryDummy" };
            //CountryRetrieveViewModel model = new CountryRetrieveViewModel() { ServiceURL = "http://www.geognos.com/api/en/countries/info/all.json" };
            return View(model);
        }

        [HttpPost]
        public IActionResult RetrieveCountries(CountryRetrieveViewModel model)
        {
            try
            {
                //RetrieveCountriesWaitable(model);

                System.Uri.TryCreate(model.ServiceURL, System.UriKind.RelativeOrAbsolute, result: out System.Uri uri);
                model.Result = _countryservice.RetrieveCountries(uri).Result;

                if (model.Result)
                {
                    ViewData["messageSucess"] = "COUNTRIES RETRIEVED SUCESSFULLY";
                }
                else
                {
                    ViewData["messageError"] = "ERROR RETRIEVING COUNTRIES!";
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return View();
        }

        #region API calls

        [Route("api/[controller]/[action]")]
        [HttpGet()]
        [AllowAnonymous]
        [Produces("application/json")]
        public IActionResult GetCountries()
        {
            return new ObjectResult(GetCountryIndexModel(null, -1).Result.CountryList);
        }

        #endregion API calls
    }
}