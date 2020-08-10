using FrameworkBaseService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using WebFrameworkBase.Models;

namespace WebFrameworkBase.TagHelpers
{
    [HtmlTargetElement("llv")]
    public class LocalizationTagHelper : TagHelper
    {
        private readonly ILocalizationService _localizationservice;
        private readonly IHttpContextAccessor _httpcontexaccessor;
        UserManager<ApplicationUser> _usermanager;

        public LocalizationTagHelper
            (
             ILocalizationService localizationservice,
             IHttpContextAccessor httpcontexaccessor,
             UserManager<ApplicationUser> usermanager
            )
        {
            _localizationservice = localizationservice;
            _httpcontexaccessor = httpcontexaccessor;
            _usermanager = usermanager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";// Replaces <localization> with empty tag

            //var valueofAtrribute = context.AllAttributes["attribute"].Value;
            var content = await output.GetChildContentAsync();


            var target = _localizationservice.GetLocalizedValue(content.GetContent(), GetApplicationUserLanguageCode());
            //output.Attributes.SetAttribute("attribute", "myvalue");
            //output.Content.SetHtmlContent(target);
            output.Content.SetContent(target);
        }

        protected string GetApplicationUserLanguageCode()
        {
            try
            {
                return _usermanager.FindByNameAsync(_httpcontexaccessor.HttpContext.User.Identity.Name).Result?.UserLanguageCode;
            }
            catch (System.Exception)
            {
                return "";
            }

        }
    }
}