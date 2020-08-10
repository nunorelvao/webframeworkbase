using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebFrameworkBase.Identity;
using WebFrameworkBase.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace WebFrameworkBase
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"FrameworkBaseSettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            var configurationLogger = new ConfigurationBuilder()
                .AddJsonFile(System.IO.Path.Combine(env.ContentRootPath, "appsettings.json"))
                .Build();



            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string DBConnectionType = Configuration["DBConnectionType"];
            switch (DBConnectionType)
            {
                case "MYSQL":
                    // Add framework services MySQL.
                    FrameworkBaseService.Initializers.ContextInitializer.InitiateMySQL(services, Configuration);
                    break;

                case "SQL":
                    // Add framework services SQL.
                    FrameworkBaseService.Initializers.ContextInitializer.InitiateSQL(services, Configuration);
                    break;

                case "SQLMEM":
                    // Add framework services SQL.
                    FrameworkBaseService.Initializers.ContextInitializer.InitiateSQLInMemory(services, Configuration);
                    break;

                default:

                    break;
            }

            // Add application services.
            FrameworkBaseService.Initializers.ServicesInitializer.Initiate(services);

            // Add identity types
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders();
            services.AddTransient<IUserStore<Models.ApplicationUser>, UserStore>();
            services.AddTransient<IRoleStore<Models.ApplicationRole>, RoleStore>();

            services.AddScoped<IPasswordHasher<ApplicationUser>, UserStore>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = System.TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = System.TimeSpan.FromDays(150);
                options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication()
            .AddFacebook(options =>
            {
                options.AppId = Configuration["auth:facebook:appid"];
                options.AppSecret = Configuration["auth:facebook:appsecret"];
                //options.Fields.Add("profile_pic");
            });



            services.AddMvc();


            services.AddDistributedMemoryCache();

            services.AddHttpContextAccessor();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(1);
                options.Cookie.Name = ".WebFrameworkBase.Session";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.HttpOnly = true;
            });


            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "WebFrameworkBase API",
                    Description = "WebFrameworkBase ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Nuno Relvão", Email = "nunorelvao@gmail.com" },
                    License = new License { Name = "Nuno Relvão", Url = "https://example.com/license" }
                });
                c.SwaggerDoc("v2", new Info
                {
                    Version = "v2",
                    Title = "WebFrameworkBase API",
                    Description = "WebFrameworkBase ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Nuno Relvão", Email = "nunorelvao@gmail.com" },
                    License = new License { Name = "Nuno Relvão", Url = "https://example.com/license" }
                });

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseSession();


            //NGINX needed
            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});

            app.UseAuthentication();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "BO",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                // routes.MapAreaRoute("api", "BO", "api/{controller}/{action}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebFrameworkBase API V1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "WebFrameworkBase API V2");
            });


        }
    }
}