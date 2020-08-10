using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebFrameworkBase.Models;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using FrameworkBaseService.Interfaces;
using System.Linq;
using System.Globalization;

namespace WebFrameworkBase.Identity
{
    public class UserStore : IUserStore<ApplicationUser>,
                        IUserClaimStore<ApplicationUser>,
                        IUserLoginStore<ApplicationUser>,
                        IUserRoleStore<ApplicationUser>,
                        IUserPasswordStore<ApplicationUser>,
                        IUserSecurityStampStore<ApplicationUser>,
                        IUserEmailStore<ApplicationUser>,
                        IPasswordHasher<ApplicationUser>
    {
        // Flag: Has Dispose already been called?
        private bool disposed = false;

        // Instantiate a SafeHandle instance.
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        private readonly IUserService _userservice;

        public UserStore(IUserService userservice)
        {
            _userservice = userservice;
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        /// <summary>
        ///     Used to generate public API error messages
        /// </summary>
        public IdentityErrorDescriber ErrorDescriber { get; set; }

        #region IUserStore

        Task<IdentityResult> IUserStore<ApplicationUser>.CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            try
            {
                int result = _userservice.CreateUser(user.UserName, user.UserPassword, user.UserRoleName, user.FirstName, user.MiddleName, user.LastName).Result;
                FrameworkBaseData.Models.User newuser = _userservice.FindUserById(result).Result;
                FillApplicationUser(user, newuser);

                return GetResult(result, null);
            }
            catch (Exception ex)
            {
                return GetResult(-1, ex);
            }
        }

        Task<IdentityResult> IUserStore<ApplicationUser>.UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            try
            {
                int result = _userservice.UpdateUser(user.Id, user.UserName, user.UserPassword, user.UserRoleName, user.FirstName, user.MiddleName, user.LastName).Result;
                FrameworkBaseData.Models.User newuser = _userservice.FindUserById(result).Result;
                FillApplicationUser(user, newuser);
                return GetResult(result, null);
            }
            catch (Exception ex)
            {
                return GetResult(-1, ex);
            }
        }

        Task<IdentityResult> IUserStore<ApplicationUser>.DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            try
            {
                bool result = _userservice.DeleteUser(user.Id).Result;
                return GetResult(1, null);
            }
            catch (Exception ex)
            {
                return GetResult(-1, ex);
            }
        }

        Task<ApplicationUser> IUserStore<ApplicationUser>.FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            ApplicationUser user = null;

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            try
            {
                int.TryParse(userId, out int id);
                FrameworkBaseData.Models.User result = _userservice.FindUserById(id).Result;
                user = FillApplicationUser(user, result);

                if (!string.IsNullOrEmpty(user.Email))
                {
                    user.EmailConfirmed = true;
                }
                user.PhoneNumber = _userservice.GetUserPhone(user.Id).Result;

                if (!string.IsNullOrEmpty(user.PhoneNumber))
                {
                    user.PhoneNumberConfirmed = true;
                }

                return Task.FromResult(user);
            }
            catch (Exception)
            {
                return Task.FromResult(user);
            }
        }

        Task<ApplicationUser> IUserStore<ApplicationUser>.FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            ApplicationUser user = null;

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            try
            {
                FrameworkBaseData.Models.User result = _userservice.FindUserByUserName(normalizedUserName).Result;
                user = FillApplicationUser(user, result);

                if (!string.IsNullOrEmpty(user.Email))
                {
                    user.EmailConfirmed = true;
                }
                user.PhoneNumber = _userservice.GetUserPhone(user.Id).Result;

                if (!string.IsNullOrEmpty(user.PhoneNumber))
                {
                    user.PhoneNumberConfirmed = true;
                }

                return Task.FromResult(user);
            }
            catch (Exception)
            {
                return Task.FromResult(user);
            }
        }

        Task<string> IUserStore<ApplicationUser>.GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.NormalizedUserName);
        }

        Task<string> IUserStore<ApplicationUser>.GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.Id.ToString());
        }

        Task<string> IUserStore<ApplicationUser>.GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.UserName);
        }

        Task IUserStore<ApplicationUser>.SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        Task IUserStore<ApplicationUser>.SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.UserName = userName;
            return Task.FromResult(0);
        }

        #endregion IUserStore

        #region IUserClaimStore NOT IMPL

        //CLAIMS ARE ADITIONAL SECURITY LEVELS EG: (BITHDATE FOR USER , ID CARD NUMBER FOR USER) KEY VALUE PAIR
        public Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (claims == null)
            {
                throw new ArgumentNullException("claims");
            }

            IDictionary<string, string> listClaims = new Dictionary<string, string>();


            //TODO: Check if claim already exists in DB, if so update values! and do not set to add (remove from list)


            //if (result == false)
            //{
            //    return Task.FromResult(0);
            //}

            //List of Claims to be added
            claims.ToList().ForEach(c => listClaims.Add(c.Type, c.Value));
            //add claims
            bool result = _userservice.AddUserClaims(user.Id, listClaims).Result;

            if (result == false)
            {
                return Task.FromResult(0);
            }
            return Task.FromResult(0);
        }

        public Task<IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            IList<Claim> listClaims = new List<Claim>();

            var result = _userservice.FindClaimsByUser(user.Id).Result;

            result.ToList().ForEach(c => listClaims.Add(new Claim(c.ClaimType, c.ClaimValue)));

            return Task.FromResult(listClaims);
        }

        public Task<IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            IList<ApplicationUser> listApplicationUsers = new List<ApplicationUser>();

            var result = _userservice.FindUsersByClaim(claim.Type).Result;

            result.ToList().ForEach(u => listApplicationUsers.Add(FillApplicationUser(new ApplicationUser(), u)));

            return Task.FromResult(listApplicationUsers);
        }

        public Task RemoveClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (claims == null)
            {
                throw new ArgumentNullException("claims");
            }

            List<string> claimsTypes = new List<string>();
            claims.ToList().ForEach(c => claimsTypes.Add(c.Type));
            return Task.FromResult(_userservice.RemoveUserClaims(user.Id, claimsTypes));
        }

        public Task ReplaceClaimAsync(ApplicationUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            if (newClaim == null)
            {
                throw new ArgumentNullException("newClaim");
            }

            return Task.FromResult(_userservice.UpdateUserClaim(user.Id, claim.Type, newClaim.ValueType, newClaim.Value));
        }

        #endregion IUserClaimStore NOT IMPL

        #region IUserLoginStore

        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            var l = new IdentityUserLogin<string>
            {
                UserId = user.Id.ToString(),
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName
            };

            try
            {
                return Task.FromResult(_userservice.AddUserLoginProvider(user.Id, login.LoginProvider, login.ProviderKey));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task RemoveLoginAsync(ApplicationUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            try
            {
                return Task.FromResult(_userservice.RemoveUserLoginProvider(user.Id, loginProvider, providerKey));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            IList<UserLoginInfo> userloginInfos = null;

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            try
            {
                List<FrameworkBaseData.Models.UserProvider> userProviders = _userservice.GetAllUserProviders(user.Id).ToList();
                userProviders.ForEach(up =>
               {
                   userloginInfos.Add(new UserLoginInfo(up.ProviderName, up.ProviderKey, up.ProviderDisplayName));
               });

                return Task.FromResult(userloginInfos);
            }
            catch (Exception)
            {
                return Task.FromResult(userloginInfos);
            }
        }

        public Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            ApplicationUser user = null;

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            try
            {
                FrameworkBaseData.Models.User result = _userservice.FindUserByLoginProvider(loginProvider, providerKey).Result;
                user = FillApplicationUser(user, result);

                if (!string.IsNullOrEmpty(user.Email))
                {
                    user.EmailConfirmed = true;
                }
                user.PhoneNumber = _userservice.GetUserPhone(user.Id).Result;

                if (!string.IsNullOrEmpty(user.PhoneNumber))
                {
                    user.PhoneNumberConfirmed = true;
                }

                return Task.FromResult(user);
            }
            catch (Exception)
            {
                return Task.FromResult(user);
            }
        }

        #endregion IUserLoginStore

        #region IUserRoleStore

        public Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (String.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException(Resources.ValueCannotBeNullOrEmpty, "roleName");
            }

            var roleEntity = _userservice.FindRoleByName(roleName).Result;
            if (roleEntity == null)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Resources.RoleNotFound, roleName));
            }

            bool result = _userservice.SetUserRole(user.Id, roleName).Result;

            if (result == false)
            {
                return Task.FromResult(0);
            }
            return Task.FromResult(0);
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(_userservice.RemoveUserRole(user.Id, roleName).Result);
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(_userservice.GetUserRolesNames(user.Id).Result);
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (roleName == null)
            {
                throw new ArgumentNullException("roleName");
            }

            return Task.FromResult(_userservice.IsUserInRole(user.Id, roleName).Result);
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (roleName == null)
            {
                throw new ArgumentNullException("roleName");
            }

            IList<ApplicationUser> listAplicationUsers = new List<ApplicationUser>();
            foreach (var user in _userservice.GetUsersInRole(roleName).Result)
            {
                ApplicationUser apUser = new ApplicationUser();
                FillApplicationUser(apUser, user);
                listAplicationUsers.Add(apUser);
            }

            return Task.FromResult(listAplicationUsers);
        }

        #endregion IUserRoleStore

        #region IUserPasswordStore

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PasswordHash != null);
        }

        #endregion IUserPasswordStore

        #region IUserSecurityStampStore

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.SecurityStamp);
        }

        #endregion IUserSecurityStampStore

        #region IUserEmailStore

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            ApplicationUser user = null;

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            try
            {
                FrameworkBaseData.Models.User result = _userservice.FindUserByUserName(normalizedEmail).Result;
                user = FillApplicationUser(user, result);

                if (!string.IsNullOrEmpty(user.Email))
                {
                    user.EmailConfirmed = true;
                }
                user.PhoneNumber = _userservice.GetUserPhone(user.Id).Result;

                if (!string.IsNullOrEmpty(user.PhoneNumber))
                {
                    user.PhoneNumberConfirmed = true;
                }

                return Task.FromResult(user);
            }
            catch (Exception)
            {
                return Task.FromResult(user);
            }
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        #endregion IUserEmailStore

        #region IPasswordHasher

        public string HashPassword(ApplicationUser user, string password)
        {
            string hashedpassword = _userservice.HashPassword(password).Result;
            user.PasswordHash = hashedpassword;
            return hashedpassword;
        }

        public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            bool result = _userservice.VerifyHashedPassword(hashedPassword, providedPassword).Result;
            bool sameHash = user.PasswordHash == hashedPassword;
            if (result == true & sameHash)
            {
                return PasswordVerificationResult.Success;
            }
            else if (result == false & sameHash)
            {
                return PasswordVerificationResult.Failed;
            }
            else if (result == true & sameHash == false)
            {
                return PasswordVerificationResult.SuccessRehashNeeded;
            }
            else
            {
                return PasswordVerificationResult.Failed;
            }
        }

        #endregion IPasswordHasher

        public class Resources
        {
            public static string ValueCannotBeNullOrEmpty = "Value cannot be null or empty";
            public static string RoleNotFound = "Role not found";
        }

        private Task<IdentityResult> GetResult(int result, Exception ex)
        {
            if (result > 0)
            {
                return System.Threading.Tasks.Task.FromResult(IdentityResult.Success);
            }
            else
            {
                if (ex != null)
                {
                    return System.Threading.Tasks.Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = "UserStore Error", Description = ex.Message }));
                }
                else
                {
                    return System.Threading.Tasks.Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = "UserStore Error", Description = "ERROR PERSISTING ON USERSERVICE" }));
                }
            }
        }

        private void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private ApplicationUser FillApplicationUser(ApplicationUser user, FrameworkBaseData.Models.User result)
        {
            Random r = new Random();
            int rSStamp = r.Next(int.MinValue, int.MaxValue);

            if (user == null)
            {
                user = new ApplicationUser();
            }


            user.Id = result.Id;
            user.UserName = result.Username;
            user.NormalizedUserName = result.Username;
            user.UserPassword = result.Userpassword;
            user.PasswordHash = result.Userpasswordhash;
            user.UserRoleName = result.Role.Name;
            user.FirstName = result.Person?.Firstname;
            user.MiddleName = result.Person?.Middlename;
            user.LastName = result.Person?.Lastname;
            user.UserLanguageCode = result.Person?.Language?.Code;
            user.UserSettings = result.UserSettings?.ToList();
            user.Email = result.Username; //_userservice.GetUserEmail(user.Id).Result;
            user.NormalizedEmail = user.Email;
            user.SecurityStamp = rSStamp.ToString();
            return user;
        }

        //Example Implementation
        //https://github.com/mhowlett/AspNet.Identity.Providers/blob/master/AspNet.Identity.Providers.Filesystem/UserStore.cs

        //Other docs
        //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-custom-storage-providers

        //https://github.com/aspnet/Docs/blob/master/aspnetcore/security/authentication/identity-custom-storage-providers/sample/CustomIdentityProviderSample/CustomProvider/CustomUserStore.cs
    }
}