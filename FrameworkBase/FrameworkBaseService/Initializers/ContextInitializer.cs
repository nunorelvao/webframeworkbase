using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FrameworkBaseData.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Reflection;
using System.Data.SqlClient;

namespace FrameworkBaseService.Initializers
{
    public static class ContextInitializer
    {
        private static readonly Serilog.ILogger _logger = Log.ForContext(MethodBase.GetCurrentMethod().DeclaringType);

        public static void InitiateSQLInMemory(IServiceCollection services, IConfigurationRoot Configuration)
        {
            services.AddDbContext<FrameworkBaseRepo.FrameworkBaseContext>(options => options.UseInMemoryDatabase("WebFrameworkBase"));
        }

        public static void InitiateSQL(IServiceCollection services, IConfigurationRoot Configuration)
        {
            services.AddDbContext<FrameworkBaseRepo.FrameworkBaseContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionSQL"), o => { o.UseRowNumberForPaging(); o.MigrationsAssembly("WebFrameworkBase"); }));
        }

        public static void InitiateMySQL(IServiceCollection services, IConfigurationRoot Configuration)
        {
            services.AddDbContext<FrameworkBaseRepo.FrameworkBaseContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnectionMYSQL"), o => { o.MigrationsAssembly("WebFrameworkBase"); }));
        }

        public static void Initialize(FrameworkBaseRepo.FrameworkBaseContext context)
        {
            bool.TryParse(context.Database.IsSqlServer().ToString(), out bool isSQL);
            bool.TryParse(context.Database.IsMySql().ToString(), out bool isMySQL);
            bool isDBCreated = true;

            if (isMySQL || isSQL)
            {
                context.Database.Migrate();
                _logger.Debug("Initialize - Database Migrations Applied!");

                isDBCreated = (context.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();
            }
            else
            {

            }
            //If DB is Created Add Seed Data...
            if (isDBCreated)
            {
                try
                {

                    _logger.Debug("Initialize - Database Type: isSQL = " + isSQL + " , isMySQL = " + isMySQL);

                    List<string> ListOfTablesToDelete = new List<string>()
                    {
                         "ContactTypes", "DocumentTypes", "AddressTypes","Countries","UserProviders", "UserClaims", "UserSettings", "Roles", "Users",
                         "PersonContacts", "PersonDocuments", "PersonAddresses","Persons",
                         "Currencies", "Localizations", "Languages"
                    };

                    if (isSQL)
                    {
                        foreach (var tablename in ListOfTablesToDelete)
                        {
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
                            context.Database.ExecuteSqlCommand(string.Format("DELETE FROM [{0}];DBCC CHECKIDENT ('{1}',RESEED, 1)", tablename, tablename));// DBCC CHECKIDENT ('{1}',RESEED, 1)", tablename, tablename);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
                        }
                    }
                    else if (isMySQL)
                    {
                        //context.Database.ExecuteSqlCommand("SET FOREIGN_KEY_CHECKS = 0;");
                        foreach (var tablename in ListOfTablesToDelete)
                        {
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
                            context.Database.ExecuteSqlCommand(string.Format("SET FOREIGN_KEY_CHECKS = 0;TRUNCATE TABLE {0};SET FOREIGN_KEY_CHECKS = 1;", tablename));
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
                        }
                        //context.Database.ExecuteSqlCommand("SET FOREIGN_KEY_CHECKS = 1;");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Initialize - Database TRUNCATE ERROR: " + ex.Message);
                }

                //ADD LANGAUGES
                var Languages = new Language[]
                   {
                    new Language{Base_Addeddate = DateTime.Now, Base_Modifieddate = DateTime.Now, Base_Ipaddress = "0.0.0.0", Base_Username = "Intial DB", Base_Enabled = true, Code= "FR", Name = "FRENCH"},
                    new Language{Base_Addeddate = DateTime.Now, Base_Modifieddate = DateTime.Now, Base_Ipaddress = "0.0.0.0", Base_Username = "Intial DB", Base_Enabled = true, Code= "EN", Name = "ENGLISH"},
                    new Language{Base_Addeddate = DateTime.Now, Base_Modifieddate = DateTime.Now, Base_Ipaddress = "0.0.0.0", Base_Username = "Intial DB", Base_Enabled = true, Code= "PT", Name = "PORTUGUESE"}
                   };
                foreach (Language l in Languages)
                {
                    context.Languages.Add(l);
                }
                context.SaveChanges();

                //ADD LOCALIZATIONS
                var languageFR = context.Languages.AsNoTracking().FirstAsync(l => l.Code == "FR").Result;
                var languagePT = context.Languages.AsNoTracking().FirstAsync(l => l.Code == "PT").Result;
                var languageEN = context.Languages.AsNoTracking().FirstAsync(l => l.Code == "EN").Result;
                var Localizations = new Localization[]
                   {
                    new Localization{Base_Addeddate = DateTime.Now, Base_Modifieddate = DateTime.Now, Base_Ipaddress = "0.0.0.0", Base_Username = "Intial DB", Base_Enabled = true,Language = languageFR, Localizationkey = "HELLO", Localizationvalue = "ALO" },
                    new Localization{Base_Addeddate = DateTime.Now, Base_Modifieddate = DateTime.Now, Base_Ipaddress = "0.0.0.0", Base_Username = "Intial DB", Base_Enabled = true,Language = languageFR, Localizationkey = "WORLD", Localizationvalue = "MONDE" },
                    new Localization{Base_Addeddate = DateTime.Now, Base_Modifieddate = DateTime.Now, Base_Ipaddress = "0.0.0.0", Base_Username = "Intial DB", Base_Enabled = true,Language = languageFR, Localizationkey = "PLEASE WAIT", Localizationvalue = "ATENDEX S'IL VOUS PLAIT" },
                    new Localization{Base_Addeddate = DateTime.Now, Base_Modifieddate = DateTime.Now, Base_Ipaddress = "0.0.0.0", Base_Username = "Intial DB", Base_Enabled = true,Language = languageFR, Localizationkey = "WAIT", Localizationvalue = "ATENDEZ" },
                    new Localization{Base_Addeddate = DateTime.Now, Base_Modifieddate = DateTime.Now, Base_Ipaddress = "0.0.0.0", Base_Username = "Intial DB", Base_Enabled = true,Language = languagePT, Localizationkey = "HELLO", Localizationvalue = "OLA" },
                    new Localization{Base_Addeddate = DateTime.Now, Base_Modifieddate = DateTime.Now, Base_Ipaddress = "0.0.0.0", Base_Username = "Intial DB", Base_Enabled = true,Language = languagePT, Localizationkey = "WORLD", Localizationvalue = "MUNDO" },
                    new Localization{Base_Addeddate = DateTime.Now, Base_Modifieddate = DateTime.Now, Base_Ipaddress = "0.0.0.0", Base_Username = "Intial DB", Base_Enabled = true,Language = languagePT, Localizationkey = "PLEASE WAIT", Localizationvalue = "POR FAVOR AGUARDE" },
                    new Localization{Base_Addeddate = DateTime.Now, Base_Modifieddate = DateTime.Now, Base_Ipaddress = "0.0.0.0", Base_Username = "Intial DB", Base_Enabled = true,Language = languagePT, Localizationkey = "WAIT", Localizationvalue = "AGUARDE" }
                   };
                foreach (Localization lz in Localizations)
                {
                    context.Localizations.Add(lz);
                }

                foreach (var documentType in Tools.Enums.GetAllValues<UserService.DocumentTypeEnum>())
                {
                    DocumentType dt = new DocumentType()
                    {
                        Base_Addeddate = DateTime.Now,
                        Base_Modifieddate = DateTime.Now,
                        Base_Ipaddress = "0.0.0.0",
                        Base_Username = "Initial DB",
                        Base_Enabled = true,
                        Code = Tools.Enums.GetEnumValue(documentType).ToString(),
                        Name = Tools.Enums.GetEnumValue(documentType).ToString() + " VALUE"
                    };
                    context.DocumentTypes.Add(dt);
                }


                foreach (var contactType in Tools.Enums.GetAllValues<UserService.ContactTypeEnum>())
                {
                    ContactType ct = new ContactType()
                    {
                        Base_Addeddate = DateTime.Now,
                        Base_Modifieddate = DateTime.Now,
                        Base_Ipaddress = "0.0.0.0",
                        Base_Username = "Initial DB",
                        Base_Enabled = true,
                        Code = Tools.Enums.GetEnumValue(contactType).ToString(),
                        Name = Tools.Enums.GetEnumValue(contactType).ToString() + " VALUE"
                    };
                    context.ContactTypes.Add(ct);
                }


                context.SaveChanges();

                foreach (var role in Tools.Enums.GetAllValues<UserService.RolesEnum>())
                {
                    Role r = new Role()
                    {
                        Base_Addeddate = DateTime.Now,
                        Base_Modifieddate = DateTime.Now,
                        Base_Ipaddress = "0.0.0.0",
                        Base_Username = "Initial DB",
                        Base_Enabled = true,
                        Code = Tools.Enums.GetEnumValue(role).ToString(),
                        Level = Tools.Enums.GetEnumValueAsInt<UserService.RolesEnum>(role),
                        Name = Tools.Enums.GetEnumValue(role).ToString()
                    };
                    context.Roles.Add(r);
                }

                context.SaveChanges();

                var roleSuperUser = context.Roles.AsNoTracking().FirstAsync(l => l.Level == 0).Result;
                //var documentTypeEmail = context.DocumentTypes.FirstOrDefaultAsync(d => d.Code == Tools.Enums.GetEnumValue(UserService.DocumentTypeEnum.EMAIL).ToString()).Result;
                //var personDocumentDB = new PersonDocument() { Base_Addeddate = DateTime.Now, Base_Modifieddate = DateTime.Now, Base_Ipaddress = "0.0.0.0", Base_Username = "Initial DB", Base_Enabled = true, Documenttype = documentTypeEmail, Value = "InitialDB@InitialDB.pt" };
                //List<PersonDocument> personDocumentsDb = new List<PersonDocument>() { personDocumentDB };
                var personDB = new Person() { Base_Addeddate = DateTime.Now, Base_Modifieddate = DateTime.Now, Base_Ipaddress = "0.0.0.0", Base_Username = "Initial DB", Base_Enabled = true, Firstname = "Initial", Lastname = "DB", Language = languageEN };
                var userDB = new User() { Base_Addeddate = DateTime.Now, Base_Modifieddate = DateTime.Now, Base_Ipaddress = "0.0.0.0", Base_Username = "Initial DB", Base_Enabled = true, Person = personDB, Role = roleSuperUser, Username = "InitialDB@InitialDB.pt", Userpassword = "InitialDB#1!", Userpasswordhash = Tools.Cryptography.GetMD5Hash("InitialDB#1!", "InitialDB#1!") };

                context.Persons.Add(personDB);
                //context.PersonDocuments.Add(personDocumentDB);
                context.Users.Add(userDB);

                context.SaveChanges();
            }
        }
    }
}