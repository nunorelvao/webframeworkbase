using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebFrameworkBase.Models;

namespace WebFrameworkBase.Areas.BO.Controllers
{
    [Area("BO")]
    [Authorize]
    public class HomeController : BaseController
    {
        private UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        //[Route("BO/Home")]
        public IActionResult Index()
        {
            return View();
        }

        //[Route("BO/About")]
        public IActionResult About()
        {
            return View();
        }
    }
}