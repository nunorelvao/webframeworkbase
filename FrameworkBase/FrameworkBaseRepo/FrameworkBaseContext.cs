using FrameworkBaseData;
using FrameworkBaseData.Models;
using FrameworkBaseData.Builders;
using Microsoft.EntityFrameworkCore;

namespace FrameworkBaseRepo
{
    public class FrameworkBaseContext : DbContext
    {
        public FrameworkBaseContext()
        {
        }

        public FrameworkBaseContext(DbContextOptions<FrameworkBaseContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder
            //    .UseSqlServer("Data Source=.\\SQLExpress;Initial Catalog=FrameworkBase;User ID=sa;Password=interlog;Trusted_Connection=True;MultipleActiveResultSets=true",
            //    x => x.MigrationsAssembly("FrameworkBaseRepo"));
            //optionsBuilder
            //    .UseMySql("Server=127.0.0.1;Database=FrameworkBase;Uid=root;Pwd=interlog;",
            //    x => x.MigrationsAssembly("FrameworkBaseRepo"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AddressType>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<Country>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<DocumentType>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<ContactType>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<Language>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<Localization>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<Person>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<PersonAddress>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<PersonDocument>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<PersonContact>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<User>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<Role>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<UserProvider>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<UserClaim>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<UserSetting>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<Currency>().Property(p => p.Base_Enabled).HasDefaultValue(true);
            modelBuilder.Entity<Currency>().Property(p => p.DecimalDigits).HasDefaultValue(0);
            modelBuilder.Entity<Currency>().Property(p => p.Rounding).HasDefaultValue(0.0);

            new AddressTypeMap(modelBuilder.Entity<AddressType>());
            new CountryMap(modelBuilder.Entity<Country>());
            new DocumentTypeMap(modelBuilder.Entity<DocumentType>());
            new ContactTypeMap(modelBuilder.Entity<ContactType>());
            new LanguageMap(modelBuilder.Entity<Language>());
            new LocalizationMap(modelBuilder.Entity<Localization>());
            new PersonMap(modelBuilder.Entity<Person>());
            new PersonAddressMap(modelBuilder.Entity<PersonAddress>());
            new PersonDocumentMap(modelBuilder.Entity<PersonDocument>());
            new PersonContactMap(modelBuilder.Entity<PersonContact>());
            new UserMap(modelBuilder.Entity<User>());
            new RoleMap(modelBuilder.Entity<Role>());
            new UserProviderMap(modelBuilder.Entity<UserProvider>());
            new UserClaimMap(modelBuilder.Entity<UserClaim>());
            new UserSettingMap(modelBuilder.Entity<UserSetting>());
            new CurrencyMap(modelBuilder.Entity<Currency>());
        }

        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Localization> Localizations { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonAddress> PersonAddresses { get; set; }
        public DbSet<PersonDocument> PersonDocuments { get; set; }
        public DbSet<PersonContact> PersonContacts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserProvider> UserProviders { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<Currency> Currencies { get; set; }

    }
}