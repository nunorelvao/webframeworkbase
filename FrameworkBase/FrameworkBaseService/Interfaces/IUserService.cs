using System;
using System.Collections.Generic;
using System.Text;
using FrameworkBaseData.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FrameworkBaseService.Interfaces
{
    public interface IUserService
    {
        IQueryable<User> GetAllUsers { get; }

        IQueryable<Role> GetAllUserRoles { get; }

        IQueryable<UserProvider> GetAllUserProviders(int userid);

        IQueryable<UserClaim> GetAllUserClaims(int userid);

        IQueryable<UserSetting> GetAllUserSettings(int userid);

        #region Role

        Task<int> CreateRole(string name, string code, int level);

        Task<int> UpdateRole(int id, string name, string code, int level);

        Task<bool> DeleteRole(int id);

        Task<Role> FindRoleById(int id);

        Task<Role> FindRoleByName(string name);

        #endregion Role

        #region User

        Task<int> CreateUser(string username, string userpassword, string userrolename);

        Task<int> CreateUser(string username, string userpassword, string userrolename, string firstname, string middlename, string lastname);

        Task<int> CreateUser(string username, string userpassword, string userrolename, string firstname, string middlename, string lastname, DateTime? borndate, Language language, List<PersonAddress> personaddresseslist, List<PersonDocument> persondocumentslist);

        Task<int> UpdateUser(int id, string username, string userpassword, string userrolename);

        Task<int> UpdateUser(int id, string username, string userpassword, string userrolename, string firstname, string middlename, string lastname);

        Task<int> UpdateUser(int id, string username, string userpassword, string userrolename, string firstname, string middlename, string lastname, DateTime? borndate, Language language, List<PersonAddress> personaddresseslist, List<PersonDocument> persondocumentslist);

        Task<bool> DeleteUser(int id);

        Task<User> FindUserById(int id);

        Task<User> FindUserByUserName(string username);

        Task<User> FindUserByPersonName(string fullnameconcatenated);

        Task<User> FindUserByPersonEmail(string email);

        Task<string> GetUserEmail(int id);

        Task<string> GetUserPhone(int id);

        Task<bool> CheckIfEmailAlreadyExists(string email);

        Task<bool> SetUserRole(int userid, string rolename);

        Task<bool> SetUserRole(int userid, int roleid);

        Task<bool> IsUserInRole(int userid, string rolename);

        Task<IQueryable<Role>> GetUserRoles(int userid);

        Task<IList<string>> GetUserRolesNames(int userid);

        Task<bool> RemoveUserRole(int userid, string rolename);

        Task<IList<User>> GetUsersInRole(int roleid);

        Task<IList<User>> GetUsersInRole(string rolename);

        #endregion User

        #region UserSettings

        Task<UserSetting> GetUserSetting(int userid, string key);

        Task<int> CreateOrUpdateUserSetting(int userid, string key, string value);

        Task<bool> UpdateUserSetting(int userid, string key, string value);

        Task<bool> UpdateUserSetting(int usersettingid, string value);

        Task<bool> DeleteUserSetting(int usersettingid);

        Task<bool> DeleteUserSetting(int userid, string key);

        #endregion

        #region LoginProvider

        Task<bool> AddUserLoginProvider(int userid, string loginProviderName, string providerKey);

        Task<bool> RemoveUserLoginProvider(int userid, string loginProviderName, string providerKey);

        Task<User> FindUserByLoginProvider(string loginProviderName, string providerKey);

        #endregion LoginProvider

        #region Claims

        Task<bool> AddUserClaims(int userid, IDictionary<string, string> clamTypesKeyValuePair);

        Task<bool> RemoveUserClaims(int userid, IList<string> clamTypes);

        Task<bool> UpdateUserClaim(int userid, string claimTypeToUpdate, string newClaimType, string newClaimValue);

        Task<IList<User>> FindUsersByClaim(string claimType);

        Task<IList<UserClaim>> FindClaimsByUser(int userid);

        #endregion Claims

        #region passwordhash

        Task<string> HashPassword(string userpassword);

        Task<bool> VerifyHashedPassword(string hashedpassword, string userpassword);

        string GetPasswordHashed(string password);

        #endregion passwordhash
    }
}