using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebFrameworkBase.Migrations
{
    public partial class INITIAL_MIGRATION_MYSQL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    Code = table.Column<string>(maxLength: 10, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    Code = table.Column<string>(maxLength: 255, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    Code = table.Column<string>(maxLength: 3, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Symbol = table.Column<string>(maxLength: 3, nullable: false),
                    SymbolNative = table.Column<string>(maxLength: 10, nullable: true),
                    DecimalDigits = table.Column<int>(nullable: false, defaultValue: 0),
                    Rounding = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    Code = table.Column<string>(maxLength: 255, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    Code = table.Column<string>(maxLength: 10, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    Code = table.Column<string>(maxLength: 10, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    Code = table.Column<string>(maxLength: 2, nullable: false),
                    Code3 = table.Column<string>(maxLength: 3, nullable: true),
                    Extcode = table.Column<string>(maxLength: 2, nullable: true),
                    Number = table.Column<int>(maxLength: 3, nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Domain = table.Column<string>(maxLength: 10, nullable: true),
                    Languageid = table.Column<int>(nullable: true),
                    Currencyid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Countries_Currencies_Currencyid",
                        column: x => x.Currencyid,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Countries_Languages_Languageid",
                        column: x => x.Languageid,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Localizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    Localizationkey = table.Column<string>(maxLength: 255, nullable: false),
                    Localizationvalue = table.Column<string>(nullable: true),
                    Languageid = table.Column<int>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Localizations_Languages_Languageid",
                        column: x => x.Languageid,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    Firstname = table.Column<string>(maxLength: 50, nullable: false),
                    Lastname = table.Column<string>(maxLength: 50, nullable: false),
                    Middlename = table.Column<string>(maxLength: 50, nullable: true),
                    Borndate = table.Column<DateTime>(nullable: true),
                    Languageid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_Languages_Languageid",
                        column: x => x.Languageid,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    Adressline1 = table.Column<string>(maxLength: 50, nullable: false),
                    Adressline2 = table.Column<string>(maxLength: 50, nullable: true),
                    Adressline3 = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<string>(maxLength: 30, nullable: true),
                    Region = table.Column<string>(maxLength: 30, nullable: true),
                    State = table.Column<string>(maxLength: 30, nullable: true),
                    Postalcode = table.Column<string>(maxLength: 10, nullable: true),
                    Countryid = table.Column<int>(nullable: false),
                    Personid = table.Column<int>(nullable: false),
                    Addresstypeid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonAddresses_AddressTypes_Addresstypeid",
                        column: x => x.Addresstypeid,
                        principalTable: "AddressTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonAddresses_Countries_Countryid",
                        column: x => x.Countryid,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonAddresses_Persons_Personid",
                        column: x => x.Personid,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonContacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    Value = table.Column<string>(maxLength: 255, nullable: false),
                    Personid = table.Column<int>(nullable: false),
                    Contacttypeid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonContacts_ContactTypes_Contacttypeid",
                        column: x => x.Contacttypeid,
                        principalTable: "ContactTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonContacts_Persons_Personid",
                        column: x => x.Personid,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    Value = table.Column<string>(maxLength: 255, nullable: false),
                    Issuedate = table.Column<DateTime>(nullable: false),
                    Expiredate = table.Column<DateTime>(nullable: false),
                    Issuelocation = table.Column<string>(nullable: true),
                    Personid = table.Column<int>(nullable: false),
                    Documenttypeid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonDocuments_DocumentTypes_Documenttypeid",
                        column: x => x.Documenttypeid,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonDocuments_Persons_Personid",
                        column: x => x.Personid,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    Username = table.Column<string>(maxLength: 50, nullable: false),
                    Userpassword = table.Column<string>(maxLength: 255, nullable: false),
                    Userpasswordhash = table.Column<string>(nullable: true),
                    Roleid = table.Column<int>(nullable: false),
                    Personid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Persons_Personid",
                        column: x => x.Personid,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Roles_Roleid",
                        column: x => x.Roleid,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    ClaimName = table.Column<string>(maxLength: 255, nullable: false),
                    ClaimValue = table.Column<string>(maxLength: 500, nullable: false),
                    ClaimType = table.Column<string>(maxLength: 255, nullable: false),
                    Userid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_Userid",
                        column: x => x.Userid,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProviders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    ProviderName = table.Column<string>(maxLength: 255, nullable: false),
                    ProviderDisplayName = table.Column<string>(maxLength: 255, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 500, nullable: false),
                    Userid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProviders_Users_Userid",
                        column: x => x.Userid,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Base_Addeddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Modifieddate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Base_Username = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Base_Ipaddress = table.Column<string>(maxLength: 255, nullable: true),
                    Base_Enabled = table.Column<bool>(nullable: true, defaultValue: true),
                    Key = table.Column<string>(maxLength: 255, nullable: false),
                    Value = table.Column<string>(maxLength: 500, nullable: false),
                    Userid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_Userid",
                        column: x => x.Userid,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Currencyid",
                table: "Countries",
                column: "Currencyid");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Languageid",
                table: "Countries",
                column: "Languageid");

            migrationBuilder.CreateIndex(
                name: "IX_Localizations_Languageid",
                table: "Localizations",
                column: "Languageid");

            migrationBuilder.CreateIndex(
                name: "IX_PersonAddresses_Addresstypeid",
                table: "PersonAddresses",
                column: "Addresstypeid");

            migrationBuilder.CreateIndex(
                name: "IX_PersonAddresses_Countryid",
                table: "PersonAddresses",
                column: "Countryid");

            migrationBuilder.CreateIndex(
                name: "IX_PersonAddresses_Personid",
                table: "PersonAddresses",
                column: "Personid");

            migrationBuilder.CreateIndex(
                name: "IX_PersonContacts_Contacttypeid",
                table: "PersonContacts",
                column: "Contacttypeid");

            migrationBuilder.CreateIndex(
                name: "IX_PersonContacts_Personid",
                table: "PersonContacts",
                column: "Personid");

            migrationBuilder.CreateIndex(
                name: "IX_PersonDocuments_Documenttypeid",
                table: "PersonDocuments",
                column: "Documenttypeid");

            migrationBuilder.CreateIndex(
                name: "IX_PersonDocuments_Personid",
                table: "PersonDocuments",
                column: "Personid");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_Languageid",
                table: "Persons",
                column: "Languageid");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_Userid",
                table: "UserClaims",
                column: "Userid");

            migrationBuilder.CreateIndex(
                name: "IX_UserProviders_Userid",
                table: "UserProviders",
                column: "Userid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Personid",
                table: "Users",
                column: "Personid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Roleid",
                table: "Users",
                column: "Roleid");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_Userid_Key",
                table: "UserSettings",
                columns: new[] { "Userid", "Key" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Localizations");

            migrationBuilder.DropTable(
                name: "PersonAddresses");

            migrationBuilder.DropTable(
                name: "PersonContacts");

            migrationBuilder.DropTable(
                name: "PersonDocuments");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserProviders");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "AddressTypes");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "ContactTypes");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
