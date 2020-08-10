using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using WebFrameworkBase.Models;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using FrameworkBaseService.Interfaces;

namespace WebFrameworkBase.Identity
{
    public class RoleStore : IRoleStore<ApplicationRole>, IQueryableRoleStore<ApplicationRole>
    {
        // Flag: Has Dispose already been called?
        private bool disposed = false;

        // Instantiate a SafeHandle instance.
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        private readonly IUserService _userservice;

        public RoleStore(IUserService userservice)
        {
            _userservice = userservice;
        }

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

        #region IRoleStore

        public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            try
            {
                int result = _userservice.CreateRole(role.Name, role.Code, role.Level).Result;
                FrameworkBaseData.Models.Role newrole = _userservice.FindRoleById(result).Result;
                FillApplicationRole(role, newrole);
                return GetResult(result, null);
            }
            catch (Exception ex)
            {
                return GetResult(-1, ex);
            }
        }

        public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            role.ConcurrencyStamp = Guid.NewGuid().ToString();
            try
            {
                int result = _userservice.UpdateRole(role.Id, role.Name, role.Code, role.Level).Result;
                FrameworkBaseData.Models.Role newrole = _userservice.FindRoleById(result).Result;
                FillApplicationRole(role, newrole);
                return GetResult(result, null);
            }
            catch (Exception ex)
            {
                return GetResult(-1, ex);
            }
        }

        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            try
            {
                bool result = _userservice.DeleteRole(role.Id).Result;
                return GetResult(1, null);
            }
            catch (Exception ex)
            {
                return GetResult(-1, ex);
            }
        }

        public Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            ApplicationRole role = new ApplicationRole();

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            try
            {
                int.TryParse(roleId, out int id);
                FrameworkBaseData.Models.Role result = _userservice.FindRoleById(id).Result;
                role.Id = result.Id;
                role.Name = result.Name;
                role.Code = result.Code;
                role.Level = result.Level;
                role.NormalizedName = result.Name;

                return Task.FromResult(role);
            }
            catch (Exception)
            {
                throw new ArgumentNullException(nameof(role));
            }
        }

        public Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            ApplicationRole role = new ApplicationRole();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            try
            {
                FrameworkBaseData.Models.Role result = _userservice.FindRoleByName(normalizedRoleName).Result;
                return Task.FromResult(FindByIdAsync(result.Id.ToString(), cancellationToken).Result);
            }
            catch (Exception)
            {
                throw new ArgumentNullException(nameof(role));
            }
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            role.Name = roleName;
            return Task.FromResult(0);
        }

        #endregion IRoleStore

        private IQueryable<ApplicationRole> GetRoles()
        {
            List<ApplicationRole> resultList = new List<ApplicationRole>();
            _userservice.GetAllUserRoles.ToList().ForEach(role =>
               resultList.Add(new ApplicationRole()
               {
                   Id = role.Id,
                   Code = role.Code,
                   Level = role.Level,
                   Name = role.Name,
                   NormalizedName = role.Name
               }
               ));
            return resultList.AsQueryable();
        }

        public IQueryable<ApplicationRole> Roles => GetRoles();

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
                    return System.Threading.Tasks.Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = "RoleStore Error", Description = ex.Message }));
                }
                else
                {
                    return System.Threading.Tasks.Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = "RoleStore Error", Description = "ERROR PERSISTING ON USERSERVICE" }));
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

        private ApplicationRole FillApplicationRole(ApplicationRole role, FrameworkBaseData.Models.Role result)
        {
            Random r = new Random();
            int rSStamp = r.Next(int.MinValue, int.MaxValue);

            if (role == null)
            {
                role = new ApplicationRole();
            }

            role.Id = result.Id;
            role.Code = result.Code;
            role.ConcurrencyStamp = rSStamp.ToString();
            role.Level = result.Level;
            role.Name = result.Name;
            role.NormalizedName = result.Name;

            return role;
        }
    }
}