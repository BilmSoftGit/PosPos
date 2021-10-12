using Pospos.Core.Common;
using Pospos.Core.Modules;
using Pospos.Data.Repositories;
using Pospos.Domain.DataTransferObjects;
using Pospos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pospos.Service
{
    public class UserService : BaseService
    {
        private readonly UserRepository _userRepository;
        public UserService(LogManager logger, UserRepository userRepository) : base(logger)
        {
            _userRepository = userRepository;
        }

        public async Task<ResponseBase<string>> GetName()
        {
            return await ExecuteAsync(() => _userRepository.GetUserName(),
                "UserService-GetName",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "GetName",
                true,
                new { asd = "asjkdaksjndk" });
        }

        public async Task<ResponseBase<Users>> UserLogin(string username, string password)
        {
            return await ExecuteAsync(() => _userRepository.GetLogin(username, password),
                "UserService-userLogin",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "userLogin",
                true,
                new { username = username, password = password });
        }

        /// <summary>
        /// Yeni girilen şifre son 5 şifreden biri ise false, yoksa true dönüyoruz...
        /// </summary>
        /// <param name="UserId">Kullanıcı Id</param>
        /// <param name="Password">Yeni şifre. Hashlenmemiş olarak girilmesi gerekmektedir.</param>
        /// <returns></returns>
        public async Task<ResponseBase<bool>> CanItBeChangedPassword(int UserId, string Password)
        {
            return await ExecuteAsync(() => _userRepository.CanItBeChangedPassword(UserId, Password),
                "UserService-CanItBeChangedPassword",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "canItBeChangedPassword",
                true,
                new { UserId = UserId, Password = Password });
        }

        /// <summary>
        /// Yeni girilen şifre son 5 şifreden biri ise false, yoksa true dönüyoruz...
        /// </summary>
        /// <param name="UserId">Kullanıcı Id</param>
        /// <param name="Password">Yeni şifre. Hashlenmemiş olarak girilmesi gerekmektedir.</param>
        /// <returns></returns>
        public async Task<ResponseBase<Users>> GetUserById(int UserId)
        {
            return await ExecuteAsync(() => _userRepository.GetById(UserId),
                "UserService-GetById",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "GetById",
                true,
                new { UserId = UserId });
        }

        public async Task<ResponseBase<bool>> UpdateUser(Users user)
        {
            return await ExecuteAsync(async () =>
            {
                return await _userRepository.UpdateUser(user);
            },
            "UserService-UpdateUser",
            ProcessTpes.Sql,
            ProcessGroup.User,
            "UpdateUser",
            true,
            user);
        }

        public async Task<ResponseBase<IEnumerable<sp_GetTopUsersWithUserId>>> GetTopUsersByUserId(int UserId, int PermissionId)
        {
            return await ExecuteAsync(async () =>
            {
                return await _userRepository.GetTopUsersByUserId(UserId, PermissionId);
            },
            "UserService-GetTopUsersByUserId",
            ProcessTpes.Sql,
            ProcessGroup.User,
            "GetTopUsersByUserId",
            true);
        }

        public async Task<ResponseBase<bool>> InsertExpiredPassword(int userId, string encryptedPass, DateTime expireDate)
        {
            return await ExecuteAsync(() => _userRepository.InsertExpiredPassword(userId, encryptedPass, expireDate),
                    "UserService-ChangePassword",
                    ProcessTpes.Sql,
                    ProcessGroup.User,
                    "changePassword-insertExpireDate",
                    true,
                    new { userId = userId, password = encryptedPass });
        }

        ///// <summary>
        ///// Kullanıcının sadece şifresini değiştiriyoruz.
        ///// </summary>
        ///// <param name="UserId">Kullanıcı Id</param>
        ///// <param name="Password">Yeni şifre. Hashlenmemiş olarak girilmesi gerekmektedir.</param>
        ///// <returns></returns>
        //public async Task<ResponseBase<bool>> changePassword(int UserId, string Password)
        //{
        //    var user = await ExecuteAsync(() => _userRepository.GetByIdAsync<Users>(UserId),
        //        "UserService-ChangePassword",
        //        ProcessTpes.Sql,
        //        ProcessGroup.User,
        //        "changePassword",
        //        true,
        //        new { userId = UserId, password = Password });

        //    if (user.Status && user != null)
        //    {
        //        var userExpiredPassword = await ExecuteAsync(() => _userRepository.InsertExpiredPassword(UserId, _utility.GetSHA512Encrypt(Password), user.data.PasswordExpireDate),
        //            "UserService-ChangePassword",
        //            ProcessTpes.Sql,
        //            ProcessGroup.User,
        //            "changePassword-insertExpireDate",
        //            true,
        //            new { userId = UserId, password = Password });

        //        if (userExpiredPassword.data)
        //        {
        //            user.data.Password = _utility.GetSHA512Encrypt(Password);
        //            user.data.PasswordExpireDate = DateTime.Now.AddDays(15);

        //            return await ExecuteAsync(async () => 
        //            {
        //                var entity = await _userRepository.GetByIdAsync<Users>(user.data.Id);
        //                if (entity != null)
        //                {
        //                    entity.EMailAddress = user.data.EMailAddress;
        //                    entity.IsActive = user.data.IsActive;
        //                    entity.InsertDate = user.data.InsertDate;
        //                    entity.Name = user.data.Name;
        //                    entity.Password = user.data.Password;
        //                    entity.PasswordExpireDate = user.data.PasswordExpireDate;
        //                    entity.Surname = user.data.Surname;
        //                    entity.Token = user.data.Token;
        //                    entity.Username = user.data.Username;
        //                    return await _userRepository.UpdateAsync<Users>(entity);
        //                }
        //                return false;
        //            },
        //                 "UserService-ChangePassword",
        //                 ProcessTpes.Sql,
        //                 ProcessGroup.User,
        //                 "changePassword-updateUser",
        //                 true,
        //                 new { userId = UserId, password = Password });
        //        }

        //    }

        //    ResponseBase<bool> result = new ResponseBase<bool>();
        //    result.data = false;
        //    return result;
        //}

        /// <summary>
        /// Kayıtlı tüm yetkileri isim sıralamasına göre getir...
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseBase<IEnumerable<Permissions>>> GetAllPermissions()
        {
            return await ExecuteAsync(() => _userRepository.GetPermissions(),
                "UserService-GetAllPermissions",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "getAllPermissions",
                true);
        }

        public async Task<ResponseBase<IEnumerable<Roles>>> GetRoles()
        {
            return await ExecuteAsync(() => _userRepository.GetRoles(),
                "UserService-GetRoles",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "GetRoles",
                true);
        }

        public async Task<ResponseBase<IEnumerable<Roles>>> GetAllRoles()
        {
            return await ExecuteAsync(() => _userRepository.GetAllRoles(),
                "UserService-GetAllRoles",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "GetAllRoles",
                true);
        }

        public async Task<ResponseBase<IEnumerable<Roles>>> GetAllRolesByUserId(int UserId)
        {
            return await ExecuteAsync(() => _userRepository.GetAllRolesByUserId(UserId),
                "UserService-GetAllRolesByUserId",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "GetAllRolesByUserId",
                true);
        }

        public async Task<ResponseBase<bool>> UpdateRole(Roles entity)
        {
            return await ExecuteAsync(async () =>
            {
                return await _userRepository.UpdateRole(entity);
            },
            "UserService-UpdateRole",
            ProcessTpes.Sql,
            ProcessGroup.User,
            "UpdateRole",
            true,
            entity);
        }

        public async Task<ResponseBase<IEnumerable<RolesPermissions>>> GetAllRolesPermissions()
        {
            return await ExecuteAsync(() => _userRepository.GetRolesPermissions(),
                "UserService-getAllPermissionRoles",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "getAllPermissionRoles",
                true);
        }

        public async Task<ResponseBase<IEnumerable<UsersRoles>>> GetAllUserRoles()
        {
            return await ExecuteAsync(() => _userRepository.GetUserRoles(),
                "UserService-GetUserRoles",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "GetUserRoles",
                true);
        }

        public async Task<ResponseBase<IEnumerable<Users>>> SearchForDataTable(int length, int page, string sortColumn, string sortColumnAscDesc, string search)
        {
            return await ExecuteAsync(() => _userRepository.SearchForDataTable(length, page, sortColumn, sortColumnAscDesc, search),
                "UserService-SearchForDataTable",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "SearchForDataTable",
                true);
        }

        public async Task<ResponseBase<IEnumerable<sp_SearchUsers>>> SearchForDataTableUsersCompany(int length, int page, string sortColumn, string sortColumnAscDesc, string search, int? CompanyId = null)
        {
            return await ExecuteAsync(() => _userRepository.SearchForDataTableUsersCompany(length, page, sortColumn, sortColumnAscDesc, search, CompanyId),
                "UserService-SearchForDataTable",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "SearchForDataTable",
                true);
        }

        public async Task<ResponseBase<IEnumerable<sp_SearchCompanies>>> CompanySearchForDataTable(int length, int page, string sortColumn, string sortColumnAscDesc, string search)
        {
            return await ExecuteAsync(() => _userRepository.CompanySearchForDataTable(length, page, sortColumn, sortColumnAscDesc, search),
                "UserService-CompanySearchForDataTable",
                ProcessTpes.Sql,
                ProcessGroup.User,
                "CompanySearchForDataTable",
                true);
        }

        public async Task<ResponseBase<IEnumerable<Company>>> GetCompanies()
        {
            return await ExecuteAsync(() => _userRepository.GetCompanies(),
                "UserService-GetCompanies",
                "Mssql",
                "User",
                "GetCompanies",
                true);
        }

        public async Task<ResponseBase<Company>> GetCompanyById(int Id)
        {
            return await ExecuteAsync(() => _userRepository.GetCompanyById(Id),
                "UserService-GetCompanies",
                "Mssql",
                "User",
                "GetCompanies",
                true);
        }

        public async Task<ResponseBase<Company>> GetCompanyByTaxNumber(string TaxNumber)
        {
            return await ExecuteAsync(() => _userRepository.GetCompanyByTaxNumber(TaxNumber),
                "UserService-GetCompaniesByTaxNumber",
                "Mssql",
                "User",
                "GetCompaniesByTaxNumber",
                true);
        }

        public async Task<ResponseBase<Company>> InsertCompany(string CompanyName, string TaxOffice, string TaxNumber, string Address, int CityId, int DistrictId, bool IsApproved, string CompanyCode)
        {
            return await ExecuteAsync(() => _userRepository.InsertCompany(CompanyName, TaxOffice, TaxNumber, Address, CityId, DistrictId, IsApproved, CompanyCode),
                "UserService-InsertCompany",
                "Mssql",
                "User",
                "InsertCompany",
                true);
        }

        public async Task<ResponseBase<Users>> GetUserByUsername(string Username)
        {
            return await ExecuteAsync(() => _userRepository.GetUserByUsername(Username),
                "UserService-GetUserByUsername",
                "Mssql",
                "User",
                "GetUserByUsername",
                true);
        }

        public async Task<ResponseBase<Users>> GetUserByEMail(string EMailAddress)
        {
            return await ExecuteAsync(() => _userRepository.GetUserByEMail(EMailAddress),
                "UserService-GetUserByEMail",
                "Mssql",
                "User",
                "GetUserByEMail",
                true);
        }

        public async Task<ResponseBase<Users>> GetUserByToken(string Token)
        {
            return await ExecuteAsync(() => _userRepository.GetUserByToken(Token),
                "UserService-GetUserByToken",
                "Mssql",
                "User",
                "GetUserByToken",
                true);
        }

        public async Task<ResponseBase<Users>> InsertUser(int CompanyId, string Username, string Password, string EMailAddress, string PhoneNumber, string Name, string Surname, bool IsActive, bool IsApproved)
        {
            return await ExecuteAsync(() => _userRepository.InsertUser(CompanyId, Username, Password, EMailAddress, PhoneNumber, Name, Surname, IsActive, IsApproved),
                "UserService-InsertUser",
                "Mssql",
                "User",
                "InsertUser",
                true);
        }

        public async Task<ResponseBase<Roles>> InsertRole(string Name, string SystemName, bool IsSystemRole)
        {
            return await ExecuteAsync(() => _userRepository.InsertRole(Name, SystemName, IsSystemRole),
                "UserService-InsertRole",
                "Mssql",
                "User",
                "InsertRole",
                true);
        }

        public async Task<ResponseBase<bool>> UpdateCompany(Company company)
        {
            return await ExecuteAsync(() => _userRepository.UpdateCompany(company),
                "UserService-UpdateCompany",
                "Mssql",
                "User",
                "UpdateCompany",
                true);
        }

        public async Task<ResponseBase<bool>> MakeUserActivePassive(int Id, bool IsActive)
        {
            return await ExecuteAsync(() => _userRepository.MakeUserActivePassive(Id, IsActive),
                "UserService-MakeUserActivePassive",
                "Mssql",
                "User",
                "MakeUserActivePassive",
                true);
        }

        public async Task<ResponseBase<bool>> MakeUserApprove(int Id, bool IsApproved)
        {
            return await ExecuteAsync(() => _userRepository.MakeUserApprove(Id, IsApproved),
                "UserService-MakeUserApprove",
                "Mssql",
                "User",
                "MakeUserApprove",
                true);
        }

        public async Task<ResponseBase<bool>> MakeCompanyApprove(int Id, bool IsApproved)
        {
            return await ExecuteAsync(() => _userRepository.MakeCompanyApprove(Id, IsApproved),
                "UserService-MakeCompanyApprove",
                "Mssql",
                "User",
                "MakeCompanyApprove",
                true);
        }

        public async Task<ResponseBase<IEnumerable<RolesPermissions>>> GetAllRolesPermissionsByRoleId(int RoleId)
        {
            return await ExecuteAsync(() => _userRepository.GetAllRolesPermissionsByRoleId(RoleId),
                "UserService-GetAllRolesPermissionsByRoleId",
                "Mssql",
                "User",
                "GetAllRolesPermissionsByRoleId",
                true);
        }

        public async Task<ResponseBase<IEnumerable<UsersRoles>>> GetAllUsersRolesByRoleId(int RoleId)
        {
            return await ExecuteAsync(() => _userRepository.GetAllUsersRolesByRoleId(RoleId),
                "UserService-GetAllUsersRolesByRoleId",
                "Mssql",
                "User",
                "GetAllUsersRolesByRoleId",
                true);
        }

        public async Task<ResponseBase<bool>> DeleteUsersRolesById(int Id)
        {
            return await ExecuteAsync(() => _userRepository.DeleteUsersRolesById(Id),
                "UserService-DeleteUsersRolesById",
                "Mssql",
                "User",
                "DeleteUsersRolesById",
                true);
        }

        public async Task<ResponseBase<bool>> DeleteUsersRolesByUserIdAndRoleId(int UserId, int RoleId)
        {
            return await ExecuteAsync(() => _userRepository.DeleteUsersRolesByUserIdAndRoleId(UserId, RoleId),
                "UserService-DeleteUsersRolesByUserIdAndRoleId",
                "Mssql",
                "User",
                "DeleteUsersRolesByUserIdAndRoleId",
                true);
        }

        public async Task<ResponseBase<bool>> DeleteRolesById(int Id)
        {
            return await ExecuteAsync(() => _userRepository.DeleteRolesById(Id),
                "UserService-DeleteRolesById",
                "Mssql",
                "User",
                "DeleteRolesById",
                true);
        }

        public async Task<ResponseBase<bool>> DeleteRolesPermissionById(int Id)
        {
            return await ExecuteAsync(() => _userRepository.DeleteRolesPermissionById(Id),
                "UserService-DeleteRolesPermissionById",
                "Mssql",
                "User",
                "DeleteRolesPermissionById",
                true);
        }

        public async Task<ResponseBase<RolesPermissions>> GetRolesPermission(int RoleId, int PermissionId)
        {
            return await ExecuteAsync(() => _userRepository.GetRolesPermission(RoleId, PermissionId),
                "UserService-GetRolesPermission",
                "Mssql",
                "User",
                "GetRolesPermission",
                true);
        }

        public async Task<ResponseBase<bool>> DeleteRolesPermissionByRolesIdAndPermissionsId(int RoleId, int PermissionId)
        {
            return await ExecuteAsync(() => _userRepository.DeleteRolesPermissionByRolesIdAndPermissionsId(RoleId, PermissionId),
                "UserService-DeleteRolesPermissionByRolesIdAndPermissionsId",
                "Mssql",
                "User",
                "DeleteRolesPermissionByRolesIdAndPermissionsId",
                true);
        }

        public async Task<ResponseBase<RolesPermissions>> InsertRolesPermissions(int RoleId, int PermissionId)
        {
            return await ExecuteAsync(() => _userRepository.InsertRolesPermissions(RoleId, PermissionId),
                "UserService-InsertRolesPermissions",
                "Mssql",
                "User",
                "InsertRolesPermissions",
                true);
        }

        public async Task<ResponseBase<UsersRoles>> InsertUserRole(int UserId, int RoleId)
        {
            return await ExecuteAsync(() => _userRepository.InsertUserRole(UserId, RoleId),
                "UserService-InsertUserRole",
                "Mssql",
                "User",
                "InsertUserRole",
                true);
        }

        public async Task<ResponseBase<Roles>> GetSystemRoleByUserId(int UserId)
        {
            return await ExecuteAsync(() => _userRepository.GetSystemRoleByUserId(UserId),
                "UserService-GetSystemRoleByUserId",
                "Mssql",
                "User",
                "GetSystemRoleByUserId",
                true);
        }

        public async Task<ResponseBase<IEnumerable<Permissions>>> GetSystemRolePermissionsByUserId(int UserId)
        {
            return await ExecuteAsync(() => _userRepository.GetSystemRolePermissionsByUserId(UserId),
                "UserService-GetSystemRolePermissionsByUserId",
                "Mssql",
                "User",
                "GetSystemRolePermissionsByUserId",
                true);
        }

        public async Task<ResponseBase<UsersLoginCode>> SetUserLoginCode(UsersLoginCode entity)
        {
            return await ExecuteAsync(() => _userRepository.SetUserLoginCode(entity),
                "UserService-SetUserLoginCode",
                "Mssql",
                "User",
                "SetUserLoginCode",
                true);
        }

        public async Task<ResponseBase<bool>> UpdateUserLoginCode(UsersLoginCode entity)
        {
            return await ExecuteAsync(() => _userRepository.UpdateUserLoginCode(entity),
                "UserService-UpdateUserLoginCode",
                "Mssql",
                "User",
                "UpdateUserLoginCode",
                true);
        }

        public async Task<ResponseBase<UsersLoginCode>> GetUserLoginCode(int UserId, string Token)
        {
            return await ExecuteAsync(() => _userRepository.GetUserLoginCode(UserId, Token),
                "UserService-GetUserLoginCode",
                "Mssql",
                "User",
                "GetUserLoginCode",
                true);
        }
    }
}
