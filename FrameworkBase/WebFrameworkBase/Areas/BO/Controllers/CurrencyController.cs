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
    public class CurrencyController : BaseController
    {
        private readonly ILocalizationService _localizationservice;
        private ILogger<CurrencyController> _logger;
        private ICurrencyService _currencyservice;

        public CurrencyController(
           ILocalizationService localizationservice,
           ICurrencyService currencyservice,
           ILogger<CurrencyController> logger,
           UserManager<ApplicationUser> usermanager) : base(usermanager)
        {
            _localizationservice = localizationservice;
            _currencyservice = currencyservice;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(GetCurrencyIndexModel(null).Result);
        }

        [HttpPost]
        public IActionResult Index(int? page)
        {
            return PartialView(GetCurrencyIndexModel(page).Result);
        }

        private async Task<CurrencyListViewModel> GetCurrencyIndexModel(int? page, int pagesize = 10)
        {
            CurrencyListViewModel model = new CurrencyListViewModel
            {
                PageSize = pagesize
            };
            var modelPaginated = _currencyservice.GetAllCurrenciesPaginated(page, model.PageSize).Result;

            modelPaginated.ForEach(c => model.CurrencyList.Add(new CurrencyViewModel()
            {
                CurrencyId = c.Id,
                CurrencyCode = c.Code,
                CurrencyName = c.Name,
                CurrencySymbol = c.Symbol,
                CurrencyNativeSymbol = c.SymbolNative,
                CurrencyDecimalDigits = c.DecimalDigits.ToString(),
                CurrencyRounding = c.Rounding.ToString("#0.00")
            }));

            ViewBag.RetrieveCurrenciesTooltip = _localizationservice.GetLocalizedValue("CURENCIESTOOLTIP", GetApplicationUserLanguageCode());

            model.HasPreviousPage = modelPaginated.HasPreviousPage;
            model.HasNextPage = modelPaginated.HasNextPage;
            model.TotalPages = modelPaginated.TotalPages;
            model.PageIndex = page ?? 1;
            return await Task.FromResult(model);
        }

        [HttpGet]
        public IActionResult RetrieveCurrencies()
        {
            CurrencyRetrieveViewModel model = new CurrencyRetrieveViewModel() { ServiceURL = "http://www.localeplanet.com/api/auto/currencymap.json" };
            return View(model);
        }

        [HttpPost]
        public IActionResult RetrieveCurrencies(CurrencyRetrieveViewModel model)
        {
            try
            {
                //RetrieveCountriesWaitable(model);

                System.Uri.TryCreate(model.ServiceURL, System.UriKind.RelativeOrAbsolute, result: out System.Uri uri);
                model.Result = _currencyservice.RetrieveCurrencies(uri).Result;

                if (model.Result)
                {
                    ViewData["messageSucess"] = "CURRENCIES RETRIEVED SUCESSFULLY";
                }
                else
                {
                    ViewData["messageError"] = "ERROR RETRIEVING CURRENCIES!";
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
        public IActionResult GetCurrencies()
        {
            return new ObjectResult(GetCurrencyIndexModel(null, -1).Result.CurrencyList);
        }

        #endregion API calls
    }
}