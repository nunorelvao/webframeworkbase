using FrameworkBaseService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using FrameworkBaseData.Models;
using System.Linq;
using Microsoft.Extensions.Logging;
using FrameworkBaseRepo;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections.Specialized;

namespace FrameworkBaseService
{
    public class UserService : IUserService
    {
        private ILogger<LocalizationService> _logger;
        private IRepository<Localization> _localizationRepository;
        private IRepository<Language> _languageRepository;
        private IRepository<User> _userRepository;
        private IRepository<Role> _userRoleRepository;
        private IRepository<UserProvider> _userProviderRepository;
        private IRepository<UserClaim> _userClaimRepository;
        private IRepository<UserSetting> _userSettingRepository;
        private IRepository<Person> _personRepository;
        private IRepository<PersonAddress> _personAdressRepository;
        private IRepository<PersonDocument> _personDocumentRepository;
        private IRepository<DocumentType> _documentTypeRepository;
        private IRepository<PersonContact> _personContactRepository;
        private IRepository<ContactType> _contactTypeRepository;

        public UserService(
            ILogger<LocalizationService> logger,
            IRepository<Localization> localizationRepository,
            IRepository<Language> languageRepository,
            IRepository<User> userRepository,
            IRepository<Role> userRoleRepository,
            IRepository<UserProvider> userProviderRepository,
            IRepository<UserClaim> userClaimRepository,
            IRepository<UserSetting> userSettingRepository,
            IRepository<Person> personRepository,
            IRepository<PersonAddress> personAdressRepository,
            IRepository<PersonDocument> personDocumentRepository,
            IRepository<DocumentType> documentTypeRepository,
            IRepository<PersonContact> personContactRepository,
            IRepository<ContactType> contactTypeRepository)
        {
            this._logger = logger;
            this._localizationRepository = localizationRepository;
            this._languageRepository = languageRepository;
            this._userRepository = userRepository;
            this._userRoleRepository = userRoleRepository;
            this._userProviderRepository = userProviderRepository;
            this._userClaimRepository = userClaimRepository;
            this._userSettingRepository = userSettingRepository;
            this._personRepository = personRepository;
            this._personAdressRepository = personAdressRepository;
            this._personDocumentRepository = personDocumentRepository;
            this._documentTypeRepository = documentTypeRepository;
            this._personContactRepository = personContactRepository;
            this._contactTypeRepository = contactTypeRepository;
        }

        public IQueryable<User> GetAllUsers { get => _userRepository.GetAll(u => u.Person, u => u.Role, u => u.Person.Language, u => u.UserSettings).AsQueryable(); }

        public IQueryable<Role> GetAllUserRoles { get => _userRoleRepository.GetAll(); }

        public IQueryable<UserProvider> GetAllUserProviders(int userid) { return _userProviderRepository.GetAll(u => u.User).Where(u => u.Userid == userid).AsQueryable(); }

        public IQueryable<UserClaim> GetAllUserClaims(int userid) { return _userClaimRepository.GetAll(u => u.User).Where(u => u.Userid == userid).AsQueryable(); }

        public IQueryable<UserSetting> GetAllUserSettings(int userid) { return _userSettingRepository.GetAll(u => u.User).Where(u => u.Userid == userid).AsQueryable(); }

        #region Users

        public Task<int> CreateUser(string username, string userpassword, string userrolename)
        {
            return CreateUser(username, userpassword, userrolename, null, null, null);
        }

        public Task<int> CreateUser(string username, string userpassword, string userrolename, string firstname, string middlename, string lastname)
        {
            return CreateUser(username, userpassword, userrolename, firstname, middlename, lastname, null, null, null, null);
        }

        public Task<int> CreateUser(string username, string userpassword, string userrolename, string firstname, string middlename, string lastname, DateTime? borndate, Language language, List<PersonAddress> personaddresseslist, List<PersonDocument> persondocumentslist)
        {
            try
            {
                //Check if already exists

                User userTocheckIfExists = FindUserByUserName(username).Result;
                if (userTocheckIfExists != null)
                {
                    return Task.FromResult(-1);
                }

                Role ur = FindRoleByName(userrolename).Result;

                if (string.IsNullOrEmpty(userrolename))
                {
                    ur = FindRoleByName(Tools.Enums.GetEnumValue(UserService.RolesEnum.STDUSER).ToString()).Result;

                    if (ur == null)
                    {
                        throw new ArgumentNullException("userrolename");
                    }

                    userpassword = "NOTSET";
                }

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userpassword))
                {
                    throw new ArgumentNullException("username or userpassword");
                }

                if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname))
                {
                    throw new ArgumentNullException("firstname or lastname");
                }

                if (borndate == new DateTime())
                {
                    borndate = null;
                }

                language = language ?? _languageRepository.GetAll().FirstOrDefault(l => l.Code == "EN");

                Person p = null;

                if (firstname != null && lastname != null)
                {
                    p = new Person
                    {
                        Firstname = firstname,
                        Lastname = lastname,
                        Borndate = borndate ?? null,
                        Middlename = middlename,
                        Language = language,
                        Personaddresses = personaddresseslist,
                        Persondocuments = persondocumentslist
                    };
                }

                _personRepository.Insert(p);

                User u = new User
                {
                    Username = username,
                    Userpassword = userpassword,
                    Person = p,
                    Userpasswordhash = GetPasswordHashed(userpassword),
                    Role = ur
                };
                _userRepository.Insert(u);

                return Task.FromResult(u.Id);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(-1);
            }
        }

        public Task<int> UpdateUser(int id, string username, string userpassword, string userrolename)
        {
            return UpdateUser(id, username, userpassword, userrolename, null, null, null);
        }

        public Task<int> UpdateUser(int id, string username, string userpassword, string userrolename, string firstname, string middlename, string lastname)
        {
            return UpdateUser(id, username, userpassword, userrolename, firstname, middlename, lastname, null, null, null, null);
        }

        public Task<int> UpdateUser(int id, string username, string userpassword, string userrolename, string firstname, string middlename, string lastname, DateTime? borndate, Language language, List<PersonAddress> personaddresseslist, List<PersonDocument> persondocumentslist)
        {
            try
            {
                Role ur = FindRoleByName(userrolename).Result;

                if (string.IsNullOrEmpty(userrolename))
                {
                    ur = FindRoleByName(Tools.Enums.GetEnumValue(UserService.RolesEnum.STDUSER).ToString()).Result;

                    if (ur == null)
                    {
                        throw new ArgumentNullException("userrolename");
                    }

                    userpassword = "NOTSET";
                }

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userpassword))
                {
                    throw new ArgumentNullException("username or userpassword");
                }

                if (borndate == new DateTime())
                {
                    borndate = null;
                }

                User userToUpdate = FindUserById(id).Result;

                if (userToUpdate.Person != null)
                {
                    userToUpdate.Person.Firstname = firstname;
                    userToUpdate.Person.Lastname = lastname;
                    userToUpdate.Person.Borndate = borndate ?? null;
                    userToUpdate.Person.Middlename = middlename;
                    userToUpdate.Person.Language = language;
                    userToUpdate.Person.Personaddresses = personaddresseslist;
                    userToUpdate.Person.Persondocuments = persondocumentslist;
                }

                userToUpdate.Username = username;
                userToUpdate.Userpassword = userpassword;
                //userToUpdate.Person = userToUpdate.Person;
                userToUpdate.Userpasswordhash = GetPasswordHashed(userpassword);
                userToUpdate.Role = ur;

                _userRepository.Update(userToUpdate);

                return Task.FromResult(userToUpdate.Id);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(-1);
            }
        }

        public Task<bool> DeleteUser(int id)
        {
            try
            {
                User userToDelete = FindUserById(id).Result;
                _userRepository.Delete(userToDelete);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(false);
            }
        }

        public Task<User> FindUserById(int id)
        {
            return Task.FromResult(_userRepository.Get(id, u => u.Person, u => u.Role, u => u.Person.Language, u => u.UserSettings));
        }

        public Task<User> FindUserByUserName(string username)
        {
            return Task.FromResult(_userRepository.GetAll(u => u.Role, u => u.Person, u => u.Person.Language, u => u.UserSettings).FirstOrDefault(u => u.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase)));
        }

        /// <summary>
        /// Finds the name of the role by.
        /// </summary>
        /// <param name="name">The FULL Name = FirstName + MiddleName + + LastName</param>
        /// <returns></returns>
        public Task<User> FindUserByPersonName(string fullnameconcatenated)
        {
            return Task.FromResult(_userRepository.GetAll(u => u.Person, u => u.Role, u => u.Person.Language, u => u.UserSettings).FirstOrDefault(u =>
              (u.Person.Firstname.Trim() + u.Person.Middlename.Trim() + u.Person.Lastname.Trim()) == fullnameconcatenated.Trim()));
        }

        public Task<User> FindUserByPersonEmail(string email)
        {
            try
            {
                ContactType dtEmail = _contactTypeRepository.GetAll().FirstOrDefault(dt => dt.Code.Equals(Tools.Enums.GetEnumValue(ContactTypeEnum.EMAIL).ToString(), StringComparison.InvariantCultureIgnoreCase));
                PersonContact personContact = _personContactRepository.GetAll().FirstOrDefault(pd => pd.Contacttype == dtEmail && pd.Value.Equals(email, StringComparison.InvariantCultureIgnoreCase));
                Person person = _personRepository.GetAll(e => e.Language, e => e.Personaddresses, e => e.Persondocuments, e => e.Personcontacts).FirstOrDefault(p => p.Personcontacts.Any(pd => pd.Id == personContact.Id));
                User userResult = _userRepository.GetAll(e => e.Person, e => e.Role, u => u.Person.Language).FirstOrDefault(u => u.Person.Id == person.Id);

                userResult.Person = _personRepository.Get(userResult.Personid.Value);
                return Task.FromResult(userResult);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return null;
            }
        }

        public Task<string> GetUserEmail(int id)
        {
            try
            {
                User user = FindUserById(id).Result;

                ContactType ctEmail = _contactTypeRepository.GetAll().FirstOrDefault(dt => dt.Code.Equals(Tools.Enums.GetEnumValue(ContactTypeEnum.EMAIL).ToString(), StringComparison.InvariantCultureIgnoreCase));

                return Task.FromResult(user.Person?.Personcontacts?.FirstOrDefault(pc => pc.Contacttype == ctEmail)?.Value);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(string.Empty);
            }
        }

        public Task<string> GetUserPhone(int id)
        {
            try
            {
                User user = FindUserById(id).Result;

                ContactType ctPhone = _contactTypeRepository.GetAll().FirstOrDefault(dt => dt.Code.Equals(Tools.Enums.GetEnumValue(ContactTypeEnum.PHONE).ToString(), StringComparison.InvariantCultureIgnoreCase));

                return Task.FromResult(user.Person?.Personcontacts?.FirstOrDefault(pd => pd.Contacttype == ctPhone)?.Value);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(string.Empty);
            }
        }

        public Task<bool> CheckIfEmailAlreadyExists(string email)
        {
            ContactType ctEmail = _contactTypeRepository.GetAll().FirstOrDefault(dt => dt.Code.Equals(Tools.Enums.GetEnumValue(ContactTypeEnum.EMAIL).ToString(), StringComparison.InvariantCultureIgnoreCase));

            bool result = _personContactRepository.GetAll().Any(d => d.Contacttype == ctEmail && d.Value.Equals(email, StringComparison.InvariantCultureIgnoreCase));

            return Task.FromResult(result);
        }

        public Task<bool> SetUserRole(int userid, string rolename)
        {
            try
            {
                Role roleToAdd = FindRoleByName(rolename).Result;
                User userToChange = FindUserById(userid).Result;
                string userName = userToChange.Username;
                //If role already exist Do nothing and just log
                if (userToChange.Role == roleToAdd)
                {
                    _logger.LogWarning("User {userName} is alredy on this role {rolename}!");
                }
                else
                {
                    //Add to the role
                    userToChange.Role = roleToAdd;
                    //Update
                    _userRepository.Update(userToChange);
                }

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(false);
            }
        }

        public Task<bool> SetUserRole(int userid, int roleid)
        {
            try
            {
                Role roleToAdd = FindRoleById(roleid).Result;
                User userToChange = FindUserById(userid).Result;
                string roleName = roleToAdd.Name;
                string userName = userToChange.Username;
                //If role already exist Do nothing and just log
                if (userToChange.Role == roleToAdd)
                {
                    _logger.LogWarning("User {userName} is alredy on this role {rolename}!");
                }
                else
                {
                    //Add to the role
                    userToChange.Role = roleToAdd;
                    //Update
                    _userRepository.Update(userToChange);
                }

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(false);
            }
        }

        public Task<bool> IsUserInRole(int userid, string rolename)
        {
            try
            {
                Role roleToCheck = FindRoleByName(rolename).Result;
                User userToCheck = FindUserById(userid).Result;
                string roleName = roleToCheck.Name;
                string userName = userToCheck.Username;
                //If role already exist return true
                if (userToCheck.Roleid == roleToCheck.Id)
                {
                    _logger.LogDebug("User {userName} is assigned on this role {roleName}!");
                    return Task.FromResult(true);
                }
                else //false
                {
                    _logger.LogDebug("User {userName} is not assigned on this role {roleName}!");
                    return Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(false);
            }
        }

        public Task<IQueryable<Role>> GetUserRoles(int userid)
        {
            try
            {
                List<Role> rolesList = new List<Role>() { _userRepository.Get(userid, u => u.Person, u => u.Role, u => u.Person.Language, u => u.UserSettings)?.Role };
                return Task.FromResult(rolesList.AsQueryable());
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return null;
            }
        }

        public Task<IList<string>> GetUserRolesNames(int userid)
        {
            try
            {
                IList<IList<string>> buildlist = new List<IList<string>>();
                string returnstr = _userRepository.Get(userid, u => u.Person, u => u.Role, u => u.Person.Language, u => u.UserSettings)?.Role?.Name;
                if (string.IsNullOrWhiteSpace(returnstr))
                {
                    return null;
                }
                buildlist.Add(new List<string>() { returnstr });

                return Task.FromResult(buildlist[0]);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return null;
            }
        }

        public Task<bool> RemoveUserRole(int userid, string rolename)
        {
            try
            {
                try
                {
                    Role roleToRemove = FindRoleByName(rolename).Result;
                    User userToChange = FindUserById(userid).Result;
                    string roleName = roleToRemove.Name;
                    string userName = userToChange.Username;
                    //If role already exist Do nothing and just log
                    if (userToChange.Roleid != roleToRemove.Id)
                    {
                        _logger.LogWarning("User {userName} is not on roleid {roleName}!");
                    }
                    else
                    {
                        _logger.LogWarning("role {roleName} cannor be deleted from User {userName}!");
                        ////Add to the role
                        //userToChange.Roles.Remove(roleToRemove);
                        ////Update
                        //_userRepository.Update(userToChange);
                    }

                    return Task.FromResult(true);
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex.Message, ex);
                    return Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(false);
            }
        }

        public Task<IList<User>> GetUsersInRole(int roleid)
        {
            try
            {
                var result = _userRepository.GetAll(u => u.Person, u => u.Role, u => u.Person.Language, u => u.UserSettings)?.Where(u => u.Roleid == roleid);
                IList<User> retusers = new List<User>();
                retusers.ToList().AddRange(result);

                return Task.FromResult(retusers);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return null;
            }
        }

        public Task<IList<User>> GetUsersInRole(string rolename)
        {
            try
            {
                var userRole = FindRoleByName(rolename);
                return GetUsersInRole(userRole.Id);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return null;
            }
        }

        #endregion Users

        #region Roles

        public Task<int> CreateRole(string name, string code, int level)
        {
            try
            {
                Role ur = new Role
                {
                    Name = name,
                    Code = code,
                    Level = level
                };
                _userRoleRepository.Insert(ur);

                return Task.FromResult(ur.Id);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(-1);
            }
        }

        public Task<int> UpdateRole(int id, string name, string code, int level)
        {
            try
            {
                Role roleToUpdate = FindRoleById(id).Result;

                roleToUpdate.Name = name;
                roleToUpdate.Code = code;
                roleToUpdate.Level = level;

                _userRoleRepository.Update(roleToUpdate);
                return Task.FromResult(roleToUpdate.Id);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(-1);
            }
        }

        public Task<bool> DeleteRole(int id)
        {
            try
            {
                Role roleToDelete = FindRoleById(id).Result;
                _userRoleRepository.Delete(roleToDelete);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(false);
            }
        }

        public Task<Role> FindRoleById(int id)
        {
            try
            {
                return Task.FromResult(_userRoleRepository.Get(id));
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return null;
            }
        }

        public Task<Role> FindRoleByName(string name)
        {
            try
            {
                return Task.FromResult(_userRoleRepository.GetAll().FirstOrDefault(ur => ur.Name == name));
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return null;
            }
        }

        #endregion Roles

        #region LoginProviders

        public Task<User> FindUserByLoginProvider(string loginProviderName, string providerKey)
        {
            try
            {
                UserProvider userProvider = _userProviderRepository.GetAll(e => e.User).
                FirstOrDefault(up => up.ProviderName.Equals(loginProviderName, StringComparison.InvariantCultureIgnoreCase) &&
                providerKey.Equals(providerKey, StringComparison.InvariantCultureIgnoreCase));

                int id = userProvider.Userid;
                return FindUserById(id);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return null;
            }
        }

        public Task<bool> AddUserLoginProvider(int userid, string loginProviderName, string providerKey)
        {
            try
            {
                User user = FindUserById(userid).Result;

                if (user != null)
                {
                    UserProvider up = new UserProvider
                    {
                        ProviderDisplayName = loginProviderName,
                        ProviderName = loginProviderName,
                        ProviderKey = providerKey,
                        User = user
                    };

                    _userProviderRepository.Insert(up);

                    return Task.FromResult(true);
                }
                else
                {
                    return Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(false);
            }
        }

        public Task<bool> RemoveUserLoginProvider(int userid, string loginProviderName, string providerKey)
        {
            try
            {
                User user = FindUserById(userid).Result;
                UserProvider up = _userProviderRepository.GetAll(e => e.User).FirstOrDefault(ur => ur.Userid == userid && ur.ProviderName == loginProviderName && ur.ProviderKey == providerKey);
                _userProviderRepository.Delete(up);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(false);
            }
        }

        #endregion LoginProviders

        #region Claims

        public Task<bool> AddUserClaims(int userid, IDictionary<string, string> clamTypesKeyValuePair)
        {
            try
            {
                List<UserClaim> listofClaims = new List<UserClaim>();
                var user = FindUserById(userid).Result;
                foreach (var claim in clamTypesKeyValuePair)
                {
                    UserClaim uc = new UserClaim
                    {
                        //User = user,
                        Userid = user.Id,
                        ClaimName = claim.Key,
                        ClaimType = claim.Key,
                        ClaimValue = claim.Value
                    };
                    listofClaims.Add(uc);
                }
                _userClaimRepository.InsertRange(listofClaims);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(false);
            }
        }

        public Task<bool> RemoveUserClaims(int userid, IList<string> clamTypes)
        {
            try
            {
                var user = FindUserById(userid).Result;
                foreach (var claimType in clamTypes)
                {
                    var claimToRemove = _userClaimRepository.GetAll(c => c.User).FirstOrDefault(c => c.ClaimType == claimType && c.Userid == userid);
                    if (claimToRemove != null)
                    {
                        _userClaimRepository.Remove(claimToRemove);
                    }
                }

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(false);
            }
        }

        public Task<bool> UpdateUserClaim(int userid, string claimTypeToUpdate, string newClaimType, string newClaimValue)
        {
            try
            {
                var claimToUpdate = _userClaimRepository.GetAll(c => c.User).FirstOrDefault(c => c.ClaimType == claimTypeToUpdate && c.Userid == userid);
                if (claimToUpdate != null)
                {
                    claimToUpdate.ClaimType = newClaimType;
                    claimToUpdate.ClaimName = newClaimType;
                    claimToUpdate.ClaimValue = newClaimValue;

                    _userClaimRepository.Update(claimToUpdate);
                }

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(false);
            }
        }

        public Task<IList<User>> FindUsersByClaim(string claimType)
        {
            try
            {
                var claimToSearch = _userClaimRepository.GetAll(c => c.User).Where(c => c.ClaimType == claimType);
                if (claimToSearch != null)
                {
                    IList<User> retusers = new List<User>();

                    foreach (var claim in claimToSearch)
                    {
                        retusers.Add(claim.User);
                    }

                    return Task.FromResult(retusers);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return null;
            }
        }

        public Task<IList<UserClaim>> FindClaimsByUser(int userid)
        {
            try
            {
                var claimToSearch = _userClaimRepository.GetAll(c => c.User).Where(c => c.Userid == userid);
                if (claimToSearch != null)
                {
                    IList<UserClaim> retclaims = new List<UserClaim>();

                    foreach (var claim in claimToSearch)
                    {
                        retclaims.Add(claim);
                    }

                    return Task.FromResult(retclaims);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return null;
            }
        }

        #endregion

        #region UserSetting

        public Task<UserSetting> GetUserSetting(int userid, string key)
        {
            try
            {
                UserSetting usGet = GetAllUserSettings(userid).SingleOrDefault(us => us.Userid == userid && us.Key == key);
                return Task.FromResult(usGet);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(new UserSetting());
            }

        }

        public Task<int> CreateOrUpdateUserSetting(int userid, string key, string value)
        {
            try
            {
                int retId = -1;

                //check if already exist 
                UserSetting usGet = GetUserSetting(userid, key).Result;


                //To INSERT case ID <= 0 else UPDATE
                if (usGet == null || usGet.Id <= 0)
                {
                    UserSetting us = new UserSetting() { Userid = userid, Key = key, Value = value };
                    _userSettingRepository.Insert(us);
                    retId = us.Id;
                }
                else
                {
                    bool updatedOK = UpdateUserSetting(userid, key, value).Result;
                    if (updatedOK)
                    {
                        retId = usGet.Id;
                    }
                }


                return Task.FromResult(retId);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(-1);
            }
        }

        public Task<bool> UpdateUserSetting(int userid, string key, string value)
        {
            try
            {

                //should only return 1 a its in unique in user id and key
                UserSetting usToUpdate = GetAllUserSettings(userid).SingleOrDefault(us => us.Userid == userid && us.Key == key);

                usToUpdate.Value = value;

                _userSettingRepository.Update(usToUpdate);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(false);
            }
        }

        public Task<bool> UpdateUserSetting(int usersettingid, string value)
        {
            try
            {
                UserSetting usToUpdate = _userSettingRepository.Get(usersettingid);

                usToUpdate.Value = value;

                _userSettingRepository.Update(usToUpdate);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(false);
            }
        }

        public Task<bool> DeleteUserSetting(int usersettingid)
        {
            try
            {
                UserSetting usToDelete = _userSettingRepository.Get(usersettingid);

                _userSettingRepository.Delete(usToDelete);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(false);
            }
        }

        public Task<bool> DeleteUserSetting(int userid, string key)
        {
            try
            {
                UserSetting usToDelete = GetAllUserSettings(userid).SingleOrDefault(us => us.Userid == userid && us.Key == key);

                _userSettingRepository.Delete(usToDelete);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message, ex);
                return Task.FromResult(false);
            }
        }

        #endregion

        #region PasswordHash

        public string GetPasswordHashed(string password)
        {
            return Tools.Cryptography.GetMD5Hash(password, password);
        }

        public Task<string> HashPassword(string userpassword)
        {
            return Task.FromResult(Tools.Cryptography.GetMD5Hash(userpassword, userpassword));
        }

        public Task<bool> VerifyHashedPassword(string hashedpassword, string userpassword)
        {
            bool result = hashedpassword == Tools.Cryptography.GetMD5Hash(userpassword, userpassword);
            return Task.FromResult(result);
        }

        #endregion PasswordHash

        #region Enums

        public enum DocumentTypeEnum
        {
            CC,
            CITIZENID,
            FISCALNUMBER
        }

        public enum ContactTypeEnum
        {
            EMAIL,
            PHONE,
            FAX,
            MOBILEPHONE,
            WORKPHONE,
            WORKMOBILEPHONE
        }

        public enum RolesEnum
        {
            SUPERADMIN = 0,
            ADMIN = 1,
            EDITOR = 2,
            READER = 3,
            STDUSER = 4
        }

        #endregion Enums
    }
}