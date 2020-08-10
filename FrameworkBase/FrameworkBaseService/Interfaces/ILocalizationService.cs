using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkBaseService.Interfaces
{
    public interface ILocalizationService
    {
        string GetLocalizedValue(string key, string languagecode);

        bool SetLocalizedValue(string key, string value, string languagecode);
    }
}