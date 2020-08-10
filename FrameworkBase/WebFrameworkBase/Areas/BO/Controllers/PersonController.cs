using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FrameworkBaseService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using WebFrameworkBase.Areas.BO.Models;
using FrameworkBaseData.Models;
using Microsoft.AspNetCore.Identity;
using WebFrameworkBase.Models;
using Microsoft.AspNetCore.Http;

namespace WebFrameworkBase.Areas.BO.Controllers

{
    [Area("BO")]
    [Authorize]
    public class PersonController : BaseController
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly ILocalizationService _localizationservice;
        private ILogger<PersonController> _logger;
        private IPersonService _personnservice;
        private ILanguageService _languageservice;
        private IUserService _userservice;

        public PersonController(
        UserManager<ApplicationUser> userManager,
        ILocalizationService localizationservice,
        IPersonService personnservice,
        ILogger<PersonController> logger,
        ILanguageService languageservice,
        IUserService userservice,
        UserManager<ApplicationUser> usermanager) : base(usermanager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _localizationservice = localizationservice;
            _personnservice = personnservice;
            _languageservice = languageservice;
            _logger = logger;
            _userservice = userservice;
        }

        public IActionResult Index()
        {
            return View(GetPersonIndexModel(null).Result);
        }

        [HttpPost]
        public IActionResult Index(int? page)
        {
            return PartialView(GetPersonIndexModel(null).Result);
        }

        private async Task<PersonListViewModel> GetPersonIndexModel(int? page, int pagesize = 10)
        {
            PersonListViewModel model = new PersonListViewModel
            {
                PageSize = pagesize
            };
            var modelPaginated = _personnservice.GetAllPersonsPaginated(page, model.PageSize).Result;

            modelPaginated.ForEach(e => model.PersonList.Add(new PersonViewModel()
            {
                PersonId = e.Id,
                FirstName = e.Firstname,
                LastName = e.Lastname,
                MiddleName = e.Middlename,
                FullName = _personnservice.GetPersonFullName(e).Result,
                Borndate = e.Borndate,
                LanguageCode = e.Language?.Code,
                // UserId = e.,

                UserColorBG = _userservice.GetUserSetting(e.Id, "UserColorBG")?.Result?.Value
            }));

            ViewBag.New = _localizationservice.GetLocalizedValue("NEW", GetApplicationUserLanguageCode());

            model.HasPreviousPage = modelPaginated.HasPreviousPage;
            model.HasNextPage = modelPaginated.HasNextPage;
            model.TotalPages = modelPaginated.TotalPages;
            model.PageIndex = page ?? 1;
            return await Task.FromResult(model);
        }

        public IActionResult Edit(int id)
        {

            var person = _personnservice.GetPersonById(id).Result;

            PersonViewModel model = new PersonViewModel();

            if (person != null)
            {
                model.FirstName = person.Firstname;
                model.MiddleName = person.Middlename;
                model.LastName = person.Lastname;
                model.Borndate = person.Borndate;
                model.FullName = _personnservice.GetPersonFullName(person).Result;
                model.LanguageCode = person.Language?.Code;
                model.PersonId = person.Id;
                model.LanguageId = person.Languageid;
                model.UserId = person.User?.Id;
                model.UserColorBG = _userservice.GetUserSetting(id, "UserColorBG")?.Result?.Value;
            }

            var languagesList = _languageservice.GetAllLanguages.Select(l => new { id = l.Id, Name = l.Name + "-" + l.Code }).ToList();
            languagesList.ForEach(l =>
            {
                model.LanguagesSelectItems.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Value = l.id.ToString(), Text = l.Name });
            });



            // ViewBag.Languages = languageservice.GetAllLanguages.Select(l => new { id = l.Id, Name = l.Name + "-" + l.Code }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PersonViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Check domain [business] validations:
                // _personnservice.CheckBusinessLogic(model);
                // if ok then update
                try
                {
                    await _personnservice.UdpatePerson(model.PersonId, model.FirstName, model.MiddleName, model.LastName, model.Borndate, model.LanguageId.Value);

                    int userId = model.UserId.HasValue ? model.UserId.Value : -1;

                    if (userId != -1)
                    {
                        HttpContext.Session.SetString("UserColorBG", model.UserColorBG);


                        await _userservice.CreateOrUpdateUserSetting(userId, "UserColorBG", model.UserColorBG);
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"UserEdit ERROR: {ex}");
                }

                //if not return error
            }

            var languagesList = _languageservice.GetAllLanguages.Select(l => new { id = l.Id, Name = l.Name + "-" + l.Code }).ToList();
            languagesList.ForEach(l =>
            {
                model.LanguagesSelectItems.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Value = l.id.ToString(), Text = l.Name });
            });



            return View(model);
        }

        #region API calls

        [Route("api/[controller]/[action]/{personid}")]
        [HttpGet()]
        [AllowAnonymous]
        [Produces("application/json")]
        public IActionResult GetPersonById(int personid)
        {
            return new ObjectResult(_personnservice.GetPersonById(personid));
        }

        [Route("api/[controller]/[action]")]
        [HttpGet()]
        [AllowAnonymous]
        [Produces("application/json")]
        public IActionResult GetPersons()
        {
            return new ObjectResult(GetPersonIndexModel(null, -1).Result.PersonList);
        }

        /// <summary>
        /// Creates the person if not exists and will not return this new entity, if already exists forces POST to update
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        [Route("api/[controller]/[action]/{personid}")]
        [HttpPut()]
        [AllowAnonymous]
        [Produces("application/json")]
        public IActionResult CreatePerson(PersonViewModel person)
        {
            if (ModelState.IsValid)
            {
                //Check if does not exist by any logic needed, if not creaet new! D not
                bool isExist = false;
                if (isExist)
                {
                    //if exist update a person
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Created;
                    return UpdatePerson(person);
                }
                else
                {
                    //create new one and do not return the object created just plain HTML response code 201 (Created)
                   // Response.StatusCode = (int)System.Net.HttpStatusCode.Created;
                    return new ObjectResult(System.Net.HttpStatusCode.Created);
                }
            }
            else
            {
                return new ObjectResult("MODEL NOT VALID!");
            }
        }

       
        [Route("api/[controller]/[action]/{personid}")]
        [HttpDelete()]
        [AllowAnonymous]
        [Produces("application/json")]
        public IActionResult DeletePerson(int personid)
        {
            if (ModelState.IsValid)
            {
                bool isFound = false;

                if (isFound)
                {
                    return new ObjectResult(System.Net.HttpStatusCode.OK);
                }
                else
                {
                    return new ObjectResult(System.Net.HttpStatusCode.NotFound);
                }

            }
            else
            {
                return new ObjectResult("MODEL NOT VALID!");
            }
        }

        /// <summary>
        /// Updates the person only if not exist should return not EXIST!
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        [Route("api/[controller]/[action]")]
        [HttpPost()]
        [AllowAnonymous]
        [Produces("application/json")]
        public IActionResult UpdatePerson(PersonViewModel person)
        {
            if (ModelState.IsValid)
            {
               
                //Should return resouce updated!
                return new ObjectResult(person);
            }
            else
            {
               // return new ObjectResult("MODEL NOT VALID!");
                return new ObjectResult(System.Net.HttpStatusCode.NotModified);
            }
        }

        #endregion API calls
    }
}