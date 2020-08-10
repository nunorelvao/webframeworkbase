using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FrameworkBaseService.Interfaces;
using WebFrameworkBase.Models.HomeModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebFrameworkBase.Models;

namespace WebFrameworkBase.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILocalizationService _localizationservice;
        private readonly IEmailSender _emailsenderservice;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILocalizationService localizationservice,
            IEmailSender emailsenderservice,
            ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _localizationservice = localizationservice;
            _emailsenderservice = emailsenderservice;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult ContactSend(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                System.Threading.Tasks.Task s = _emailsenderservice.SendEmailAsync(model.EmailTo, "CONTACT FROM MYWEBFRAMEWORK", model.BodyText);
            }

            return View("Contact");
        }

        public IActionResult PrivatePolicy()
        {
            ViewData["Message"] = "Your Private Policy page.";

            ViewData["MessageLocalized"] = _localizationservice.GetLocalizedValue("HELLO", GetApplicationUserLanguageCode()) + " " + _localizationservice.GetLocalizedValue("WORLD", GetApplicationUserLanguageCode());

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}