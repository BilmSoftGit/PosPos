using Dapper;
using Pospos.Core.Modules;
using Pospos.Data.BaseRepositories;
using Pospos.Domain.DataTransferObjects;
using Pospos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Pospos.Data.Repositories
{
    public class UserRepository : CustomRepository
    {
        public UserRepository(MainConnectionManager connection) : base(connection)
        {

        }

        #region Kullanıcı İşlemleri

        public async Task<string> GetUserName()
        {
            return "Elaksdnaskdlamsd";
        }

        public async Task<Users> GetById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id);

            return await this.QeryFirstAsync<Users>("SELECT * FROM [Users] WHERE Id = @Id", parameters, CommandType.Text);
        }

        public async Task<Users> GetLogin(string Username, string Password)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Username", Username);
            parameters.Add("@Password", Password);

            return await this.QeryFirstAsync<Users>("SELECT * FROM [Users] WHERE Username = @Username AND Password = @Password", parameters, CommandType.Text);
        }

        //Yeni girilen şifre son 5 şifreden biri ise false, yoksa true dönüyoruz...
        public async Task<bool> CanItBeChangedPassword(int UserId, string Password)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserId", UserId);
            parameters.Add("@Password", Password);

            var result = await this.QeryFirstAsync<Users>("SELECT * FROM (SELECT TOP 5 * FROM [UsersExpiredPasswords] WHERE UserId = @UserId ORDER BY Id DESC) Tablo WHERE Password = @Password", parameters, CommandType.Text);

            return result == null;
        }

        public async Task<bool> InsertExpiredPassword(int UserId, string Password, DateTime ExpireDate)
        {
            UsersExpiredPasswords entity = new UsersExpiredPasswords();
            entity.ExpireDate = ExpireDate;
            entity.UserId = UserId;
            entity.Password = Password;

            return await this.InsertAsync<UsersExpiredPasswords>(entity) != null;
        }

        public async Task<bool> UpdateUser(Users user)
        {
            return await this.UpdateAsync<Users>(user);
        }

        public async Task<IEnumerable<Users>> SearchForDataTable(int length, int page, string sortColumn, string sortColumnAscDesc, string search)
        {
            DynamicParameters paramaters = new DynamicParameters();
            paramaters.Add("PageSize", length);
            paramaters.Add("CurrentPage", page);
            paramaters.Add("sortName", sortColumn);
            paramaters.Add("sortDirection", sortColumnAscDesc);
            paramaters.Add("search", search);

            return await this.QeryAsync<Users>("sp_SearchUsers", paramaters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<sp_SearchUsers>> SearchForDataTableUsersCompany(int length, int page, string sortColumn, string sortColumnAscDesc, string search, int? CompanyId = null)
        {
            DynamicParameters paramaters = new DynamicParameters();
            paramaters.Add("PageSize", length);
            paramaters.Add("CurrentPage", page);
            paramaters.Add("sortName", sortColumn);
            paramaters.Add("sortDirection", sortColumnAscDesc);
            paramaters.Add("search", search);
            paramaters.Add("companyId", CompanyId);

            return await this.QeryAsync<sp_SearchUsers>("sp_SearchUsers", paramaters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<sp_GetTopUsersWithUserId>> GetTopUsersByUserId(int UserId, int PermissionId)
        {
            DynamicParameters paramaters = new DynamicParameters();
            paramaters.Add("userId", UserId);
            paramaters.Add("permissionId", PermissionId);

            return await this.QeryAsync<sp_GetTopUsersWithUserId>("sp_GetTopUsersWithUserId", paramaters, CommandType.StoredProcedure);
        }

        public async Task<Users> GetUserByUsername(string Username)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Username", Username);

            return await this.QeryFirstAsync<Users>("SELECT * FROM [Users] WHERE Username = @Username ORDER BY Name ASC", parameters, CommandType.Text);
        }

        public async Task<Users> GetUserByEMail(string EMailAddress)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("EMailAddress", EMailAddress);

            return await this.QeryFirstAsync<Users>("SELECT * FROM [Users] WHERE EMailAddress = @EMailAddress ORDER BY Name ASC", parameters, CommandType.Text);
        }

        public async Task<Users> GetUserByToken(string Token)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Token", Token);

            return await this.QeryFirstAsync<Users>("SELECT * FROM [Users] WHERE Token = @Token ORDER BY Name ASC", parameters, CommandType.Text);
        }

        public async Task<Users> InsertUser(int CompanyId, string Username, string Password, string EMailAddress, string PhoneNumber, string Name, string Surname, bool IsActive, bool IsApproved)
        {
            Users entity = new Users();
            entity.CompanyId = CompanyId;
            entity.Username = Username;
            entity.Password = Password;
            entity.PasswordExpireDate = DateTime.Now.AddDays(15);
            entity.EMailAddress = EMailAddress;
            entity.PhoneNumber = PhoneNumber;
            entity.Name = Name;
            entity.Surname = Surname;
            entity.IsActive = IsActive;
            entity.InsertDate = DateTime.Now;
            entity.IsApproved = IsApproved;

            return await this.InsertAsync<Users>(entity);
        }

        public async Task<bool> MakeUserActivePassive(int Id, bool IsActive)
        {
            var user = await GetById(Id);
            if (user != null)
            {
                user.IsActive = IsActive;

                if (await UpdateUser(user))
                    return true;
            }

            return false;
        }

        public async Task<bool> MakeUserApprove(int Id, bool IsApproved)
        {
            var user = await GetById(Id);
            if (user != null)
            {
                user.IsApproved = IsApproved;

                if (await UpdateUser(user))
                    return true;
            }

            return false;
        }

        #endregion


        #region Company İşlemleri

        public async Task<bool> MakeCompanyApprove(int Id, bool IsApproved)
        {
            var company = await GetByIdAsync<Company>(Id);
            if (company != null)
            {
                company.IsApproved = IsApproved;

                if (await UpdateCompany(company))
                    return true;
            }

            return false;
        }

        public async Task<bool> UpdateCompany(Company company)
        {
            return await this.UpdateAsync<Company>(company);
        }

        public async Task<IEnumerable<sp_SearchCompanies>> CompanySearchForDataTable(int length, int page, string sortColumn, string sortColumnAscDesc, string search)
        {
            DynamicParameters paramaters = new DynamicParameters();
            paramaters.Add("PageSize", length);
            paramaters.Add("CurrentPage", page);
            paramaters.Add("sortName", sortColumn);
            paramaters.Add("sortDirection", sortColumnAscDesc);
            paramaters.Add("search", search);

            return await this.QeryAsync<sp_SearchCompanies>("sp_SearchCompanies", paramaters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await this.QeryAsync<Company>("SELECT * FROM [Company] ORDER BY CompanyName ASC", parameters, CommandType.Text);
        }

        public async Task<Company> GetCompanyById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", Id);

            return await this.QeryFirstAsync<Company>("SELECT * FROM [Company] WHERE Id = @Id", parameters, CommandType.Text);
        }

        public async Task<Company> GetCompanyByTaxNumber(string TaxNumber)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("TaxNumber", TaxNumber);

            return await this.QeryFirstAsync<Company>("SELECT * FROM [Company] WHERE TaxNumber = @TaxNumber", parameters, CommandType.Text);
        }

        public async Task<Company> InsertCompany(string CompanyName, string TaxOffice, string TaxNumber, string Address, int CityId, int DistrictId, bool IsApproved, string CompanyCode)
        {
            Company entity = new Company();
            entity.Address = Address;
            entity.CompanyName = CompanyName;
            entity.InsertDate = DateTime.Now;
            entity.IsApproved = IsApproved;
            entity.TaxNumber = TaxNumber;
            entity.TaxOffice = TaxOffice;
            entity.CityId = CityId;
            entity.DistrictId = DistrictId;
            entity.CompanyCode = CompanyCode;

            return await this.InsertAsync<Company>(entity);
        }

        #endregion


        #region Rol İşlemleri

        public async Task<UsersRoles> GetUsersRolesByUserIdRoleId(int UserId, int RoleId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", UserId);
            parameters.Add("RoleId", RoleId);

            return await this.QeryFirstAsync<UsersRoles>("SELECT * FROM [UsersRoles] WHERE UserId = @UserId AND RoleId = @RoleId", parameters, CommandType.Text);
        }

        public async Task<bool> DeleteUsersRolesById(int Id)
        {
            return await this.Delete<UsersRoles>(Id) > 0;
        }

        public async Task<bool> DeleteUsersRolesByUserIdAndRoleId(int UserId, int RoleId)
        {
            var entity = await GetUsersRolesByUserIdRoleId(UserId, RoleId);

            return await this.Delete<UsersRoles>(entity.Id) > 0;
        }

        public async Task<bool> DeleteRolesPermissionById(int Id)
        {
            return await this.Delete<RolesPermissions>(Id) > 0;
        }

        public async Task<bool> DeleteRolesById(int Id)
        {
            return await this.Delete<Roles>(Id) > 0;
        }

        public async Task<IEnumerable<RolesPermissions>> GetAllRolesPermissionsByRoleId(int RoleId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("RoleId", RoleId);

            return await this.QeryAsync<RolesPermissions>("SELECT * FROM [RolesPermissions] WHERE RoleId = @RoleId ORDER BY Id ASC", parameters, CommandType.Text);
        }

        public async Task<IEnumerable<UsersRoles>> GetAllUsersRolesByRoleId(int RoleId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("RoleId", RoleId);

            return await this.QeryAsync<UsersRoles>("SELECT * FROM [UsersRoles] WHERE RoleId = @RoleId ORDER BY Id ASC", parameters, CommandType.Text);
        }

        public async Task<RolesPermissions> GetRolesPermission(int RoleId, int PermissionId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("RoleId", RoleId);
            parameters.Add("PermissionId", PermissionId);

            return await this.QeryFirstAsync<RolesPermissions>("SELECT * FROM [RolesPermissions] WHERE RoleId = @RoleId AND PermissionId = @PermissionId", parameters, CommandType.Text);
        }

        public async Task<bool> DeleteRolesPermissionByRolesIdAndPermissionsId(int RoleId, int PermissionId)
        {
            var roles_permissions = await GetRolesPermission(RoleId, PermissionId);
            if (roles_permissions != null)
                return await this.Delete<RolesPermissions>(roles_permissions.Id) > 0;

            return false;
        }

        public async Task<RolesPermissions> InsertRolesPermissions(int RoleId, int PermissionId)
        {
            RolesPermissions entity = new RolesPermissions();
            entity.RoleId = RoleId;
            entity.PermissionId = PermissionId;

            return await this.InsertAsync<RolesPermissions>(entity);
        }

        public async Task<UsersRoles> InsertUserRole(int UserId, int RoleId)
        {
            UsersRoles entity = new UsersRoles();
            entity.UserId = UserId;
            entity.RoleId = RoleId;
            entity.InsertDate = DateTime.Now;

            return await this.InsertAsync<UsersRoles>(entity);
        }

        public async Task<Roles> InsertRole(string Name, string SystemName, bool IsSystemRole)
        {
            Roles entity = new Roles();
            entity.Name = Name;
            entity.SystemName = SystemName;
            entity.IsSystemRole = IsSystemRole;

            return await this.InsertAsync<Roles>(entity);
        }

        public async Task<IEnumerable<Permissions>> GetPermissions()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await this.QeryAsync<Permissions>("SELECT  * FROM [Permissions] ORDER BY Name ASC", parameters, CommandType.Text);
        }

        /// <summary>
        /// sistem rolleri hariç tüm rolleri getir
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Roles>> GetRoles()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await this.QeryAsync<Roles>("SELECT  * FROM [Roles] WHERE IsSystemRole = 0 ORDER BY Name ASC", parameters, CommandType.Text);
        }

        public async Task<bool> UpdateRole(Roles entity)
        {
            return await this.UpdateAsync<Roles>(entity);
        }

        /// <summary>
        /// Sistem rolleri de dahil tüm rolleri getir
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Roles>> GetAllRoles()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await this.QeryAsync<Roles>("SELECT  * FROM [Roles] ORDER BY Name ASC", parameters, CommandType.Text);
        }
        public async Task<IEnumerable<Roles>> GetAllRolesByUserId(int UserId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", UserId);

            return await this.QeryAsync<Roles>("SELECT R.* FROM Roles R INNER JOIN UsersRoles UR ON UR.RoleId = R.Id WHERE UR.UserId = @UserId ORDER BY Name ASC", parameters, CommandType.Text);
        }

        public async Task<IEnumerable<RolesPermissions>> GetRolesPermissions()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await this.QeryAsync<RolesPermissions>("SELECT  * FROM [RolesPermissions] ORDER BY Id ASC", parameters, CommandType.Text);
        }

        public async Task<IEnumerable<UsersRoles>> GetUserRoles()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await this.QeryAsync<UsersRoles>("SELECT  * FROM [UsersRoles] ORDER BY Id ASC", parameters, CommandType.Text);
        }

        public async Task<Roles> GetSystemRoleByUserId(int UserId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", UserId);

            return await this.QeryFirstAsync<Roles>("SELECT R.* FROM Roles R INNER JOIN UsersRoles UR ON R.Id = UR.RoleId WHERE UR.UserId = @UserId AND R.IsSystemRole = 1", parameters, CommandType.Text);
        }

        public async Task<IEnumerable<Permissions>> GetSystemRolePermissionsByUserId(int UserId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", UserId);

            return await this.QeryAsync<Permissions>("SELECT P.* FROM Roles R INNER JOIN UsersRoles UR ON UR.RoleId = R.Id INNER JOIN RolesPermissions RP ON RP.RoleId = R.Id INNER JOIN Permissions P ON P.Id = RP.PermissionId WHERE UR.UserId = @UserId AND R.IsSystemRole = 1", parameters, CommandType.Text);
        }

        #endregion

        public async Task<UsersLoginCode> SetUserLoginCode(UsersLoginCode entity)
        {   
            return await this.InsertAsync<UsersLoginCode>(entity);
        }

        public async Task<bool> UpdateUserLoginCode(UsersLoginCode entity)
        {
            return await this.UpdateAsync<UsersLoginCode>(entity);
        }

        public async Task<UsersLoginCode> GetUserLoginCode(int UserId, string Token)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", UserId);
            parameters.Add("Token", Token);
            parameters.Add("ThisDate", DateTime.Now);

            return await this.QeryFirstAsync<UsersLoginCode>("SELECT * FROM UsersLoginCode WHERE UserId = @UserId AND Token = @Token AND PassiveDate >= @ThisDate", parameters, CommandType.Text);
        }
    }
}
