using Microsoft.AspNetCore.Http;
using Pospos.Core.Common;
using Pospos.Core.Helpers;
using Pospos.Core.Modules;
using Pospos.Domain.Entities;
using Pospos.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pospos.Business.Factories
{
    public class CacheFactory
    {
        private readonly CacheManager _cacheManager;
        private readonly UserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CacheFactory(CacheManager cacheManager, UserService userService, IHttpContextAccessor httpContext)
        {
            this._cacheManager = cacheManager;
            this._userService = userService;
            this._httpContextAccessor = httpContext;
        }

        public async Task<bool> PermissionControl(string name)
        {
            var _session = SessionExtensions.Get<LoginSessionData>(_httpContextAccessor.HttpContext.Session, "UserData");
            return await GetPermission(_session.UserId, name) ? true : await GetPermission(_session.UserId, PanelPermissions.GeneralManagement);
        }

        public async Task<bool> GetPermission(int UserId, string PermissionSystemName)
        {
            if (await _cacheManager.GetAsync<IEnumerable<Permissions>>("permissions") == null)
               await SetPermissions();

            var permissions = await _cacheManager.GetAsync<IEnumerable<Permissions>>("permissions");
            var roles = await _cacheManager.GetAsync<IEnumerable<Roles>>("roles");
            var roles_permissions = await _cacheManager.GetAsync<IEnumerable<RolesPermissions>>("roles_permissions");
            var users_roles = await _cacheManager.GetAsync<IEnumerable<UsersRoles>>("user_roles");

            var user_roles = users_roles.Where(x => x.UserId == UserId);
            foreach (var user_role in user_roles)
            {
                var _permissions = roles_permissions.Where(x => x.RoleId == user_role.RoleId);
                foreach (var permission in _permissions)
                {
                    if (permissions.Where(x => x.Id == permission.PermissionId && x.SystemName == PermissionSystemName).FirstOrDefault() != null)
                        return true;
                }
            }

            return false;
        }

        public async Task<bool> SetPermissions()
        {
            try
            {
                await _cacheManager.ClearAsync();

                var permissions = await _userService.GetAllPermissions();
                var roles = await _userService.GetRoles();
                var roles_permissions = await _userService.GetAllRolesPermissions();
                var user_roles = await _userService.GetAllUserRoles();

                await _cacheManager.SetAsync<IEnumerable<Permissions>>("permissions", permissions.data);
                await _cacheManager.SetAsync<IEnumerable<Roles>>("roles", roles.data);
                await _cacheManager.SetAsync<IEnumerable<RolesPermissions>>("roles_permissions", roles_permissions.data);
                await _cacheManager.SetAsync<IEnumerable<UsersRoles>>("user_roles", user_roles.data);

                return true;
            }
            catch (Exception ex)
            { }

            return false;
        }

        public async Task<bool> ClearCache()
        {
            await _cacheManager.ClearAsync("permissions");
            await _cacheManager.ClearAsync("roles");
            await _cacheManager.ClearAsync("roles_permissions");
            await _cacheManager.ClearAsync("user_roles");

            var permissions = await _userService.GetAllPermissions();
            var roles = await _userService.GetRoles();
            var roles_permissions = await _userService.GetAllRolesPermissions();
            var user_roles = await _userService.GetAllUserRoles();

            await _cacheManager.SetAsync<IEnumerable<Permissions>>("permissions", permissions.data);
            await _cacheManager.SetAsync<IEnumerable<Roles>>("roles", roles.data);
            await _cacheManager.SetAsync<IEnumerable<RolesPermissions>>("roles_permissions", roles_permissions.data);
            await _cacheManager.SetAsync<IEnumerable<UsersRoles>>("user_roles", user_roles.data);

            //await _cacheManager.ClearAsync();

            return true;
        }

        public class MyClass
        {
            public IEnumerable<Permissions> _Permissions { get; set; }
            public IEnumerable<Roles> _Roles { get; set; }
            public IEnumerable<RolesPermissions> _RolesPermissions { get; set; }
            public IEnumerable<UsersRoles> _UsersRoles { get; set; }
        }

        public async Task<MyClass> GetPermissionList()
        {
            MyClass result = new MyClass();

            var _session = SessionExtensions.Get<LoginSessionData>(_httpContextAccessor.HttpContext.Session, "UserData");

            result._Permissions = await _cacheManager.GetAsync<IEnumerable<Permissions>>("permissions");
            result._Roles = await _cacheManager.GetAsync<IEnumerable<Roles>>("roles");
            result._RolesPermissions = await _cacheManager.GetAsync<IEnumerable<RolesPermissions>>("roles_permissions");
            result._UsersRoles = await _cacheManager.GetAsync<IEnumerable<UsersRoles>>("user_roles");

            result._UsersRoles = result._UsersRoles.Where(x => x.UserId == _session.UserId);

            return result;
        }
    }
}
