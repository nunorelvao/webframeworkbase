using Microsoft.Extensions.DependencyInjection;
using FrameworkBaseService;
using FrameworkBaseService.Interfaces;
using FrameworkBaseRepo;

namespace FrameworkBaseService.Initializers
{
    public static class ServicesInitializer
    {
        public static void Initiate(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<ILocalizationService, LocalizationService>();
            services.AddTransient<ILanguageService, LanguageService>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<IUserService, UserService>();


            services.AddTransient(typeof(IRestService<>), typeof(RestService<>));

        }
    }
}