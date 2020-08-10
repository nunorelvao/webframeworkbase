using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Configuration;

namespace WebFrameworkBase
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            var configurationFile = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
               .WriteTo.Console()
               .ReadFrom.Configuration(configurationFile)
               ////.WriteTo.RollingFile(Path.Combine(env.ContentRootPath, "log-{Date}.txt"))
               .CreateLogger();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    var environment = services.GetRequiredService<IHostingEnvironment>();
                    //Check for setting if is to creaet DB should only create once!
                    bool isToCreateDBSetting = false;
                    bool.TryParse(configurationFile.GetSection("DBCREATEONSERVER").Value, out isToCreateDBSetting);

                    logger.LogInformation(string.Format("environment.IsDevelopment(); {0}", environment.IsDevelopment()));

                    if (isToCreateDBSetting)
                    {
                        using (var context = services.GetRequiredService<FrameworkBaseRepo.FrameworkBaseContext>())
                        {
                            FrameworkBaseService.Initializers.ContextInitializer.Initialize(context);
                        }

                    }
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
            //.UseUrls("http://*:8080")
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>()
            .UseSerilog()
            .Build();
    }
}