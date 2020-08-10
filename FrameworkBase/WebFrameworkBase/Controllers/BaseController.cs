using FrameworkBaseData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebFrameworkBase.Models;

namespace Microsoft.AspNetCore.Mvc
{
    public abstract class BaseController : Controller
    {
        UserManager<ApplicationUser> _usermanager;
        //private readonly IHttpContextAccessor _httpcontexaccessor;
        // private readonly IHttpContextAccessor _httpcontexaccessor;


        public BaseController
            (
            UserManager<ApplicationUser> usermanager
            //,IHttpContextAccessor httpcontexaccessor
            ) : base()
        {

            _usermanager = usermanager;
            //_httpcontexaccessor = httpcontexaccessor;



        }

        protected string GetApplicationUserLanguageCode()
        {
            string userLanguageCode = string.Empty;
            try
            {
                userLanguageCode = _usermanager.FindByNameAsync(HttpContext.User.Identity.Name).Result?.UserLanguageCode;
                //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(userLanguageCode);
                //Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            }
            catch (System.Exception)
            {
                return userLanguageCode;
            }

            return userLanguageCode;
        }


        protected List<UserSetting> GetandSetUserSettings()
        {
            List<UserSetting> usersettingsList = new List<UserSetting>();

            try
            {
                if (HttpContext != null)
                {
                    usersettingsList = _usermanager.FindByNameAsync(HttpContext.User.Identity.Name).Result?.UserSettings;

                    if (usersettingsList != null)
                    {
                        //TRY SET BACKGROUND COLOR
                        HttpContext.Session.SetString("UserColorBG", usersettingsList.FirstOrDefault(us => us.Key == "UserColorBG").Value);
                    }
                }

            }
            catch (System.Exception)
            {
                return usersettingsList;
            }

            return usersettingsList;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            GetandSetUserSettings();

        }

    }
}
