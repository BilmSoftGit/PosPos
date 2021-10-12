using Pospos.Core.Helpers;
using Pospos.Domain.DataTransferObjects;
using Pospos.Domain.Entities;
using Pospos.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pospos.Business.Factories
{
    public class UserFactory
    {
        private readonly UserService _userService;
        private readonly Utility _utility;
        public UserFactory(UserService userService, Utility utility)
        {
            _userService = userService;
            _utility = utility;
        }

        /// <summary>
        /// Kullanıcı adı ve şifreye göre kullanıcı getirme...
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Users> UserLogin(string username, string password)
        {
            //Bu alanda şifreleme olacak...
            var user = await _userService.UserLogin(username, _utility.GetSHA512Encrypt(password));

            return user.data;
        }

        public async Task<bool> CheckUserPassword(int UserId, string Password)
        {
            var user = await _userService.GetUserById(UserId);
            if (user.data != null)
            {
                if (user.data.Password == _utility.GetSHA512Encrypt(Password))
                    return true;
            }

            return false;
        }

        public async Task<Users> GetUserById(int Id)
        {
            var user = await _userService.GetUserById(Id);

            return user.data;
        }

        public async Task<Users> GetUserByToken(string Token)
        {
            var user = await _userService.GetUserByToken(Token);

            return user.data;
        }

        /// <summary>
        /// Yeni girilen şifre son 5 şifreden biri ise false, yoksa true dönüyoruz...
        /// </summary>
        /// <param name="UserId">Kullanıcı Id</param>
        /// <param name="Password">Yeni şifre. Hashlenmemiş olarak girilmesi gerekmektedir.</param>
        /// <returns></returns>
        public async Task<bool> CanItBeChangedPassword(int UserId, string Password)
        {
            var result = await _userService.CanItBeChangedPassword(UserId, _utility.GetSHA512Encrypt(Password));

            return result.data;
        }

        /// <summary>
        /// Kullanıcının sadece şifresini değiştiriyoruz.
        /// </summary>
        /// <param name="UserId">Kullanıcı Id</param>
        /// <param name="Password">Yeni şifre. Hashlenmemiş olarak girilmesi gerekmektedir.</param>
        /// <returns></returns>
        public async Task<bool> ChangePassword(int UserId, string Password)
        {
            var user = await _userService.GetUserById(UserId);
            if (user.Status)
            {
                if (user.data != null)
                {
                    string encryptedPass = _utility.GetSHA512Encrypt(Password);
                    var userExpiredPassword = await _userService.InsertExpiredPassword(UserId, encryptedPass, user.data.PasswordExpireDate);

                    if (userExpiredPassword.data)
                    {
                        user.data.Password = encryptedPass;
                        user.data.PasswordExpireDate = DateTime.Now.AddDays(15);
                        var resp = await _userService.UpdateUser(user.data);
                        return resp.data;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Sistemdeki yetkilerin tümünü isme göre sıralayarak getirir...
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Permissions>> GetAllPermissions()
        {
            var result = await _userService.GetAllPermissions();

            return result.data;
        }
        public async Task<IEnumerable<sp_GetTopUsersWithUserId>> GetTopUsersByUserId(int UserId, int PermissionId)
        {
            var result = await _userService.GetTopUsersByUserId(UserId, PermissionId);

            return result.data;
        }

        /// <summary>
        /// Sistemdeki rollerin tümünü isme göre sıralayarak getirir...
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Roles>> GetRoles()
        {
            var result = await _userService.GetRoles();

            return result.data;
        }

        public async Task<IEnumerable<Roles>> GetAllRoles()
        {
            var result = await _userService.GetAllRoles();

            return result.data;
        }

        /// <summary>
        /// Rol ve yetkileri kayıt id'sine göre sıralayarak getirir...
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RolesPermissions>> GetAllRolesPermissions()
        {
            var result = await _userService.GetAllRolesPermissions();

            return result.data;
        }

        /// <summary>
        /// Datatable için verilen kullanıcı arama fonksiyonu
        /// </summary>
        /// <param name="length"></param>
        /// <param name="page"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortColumnAscDesc"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Users>> SearchForDataTable(int length, int page, string sortColumn, string sortColumnAscDesc, string search)
        {
            var result = await _userService.SearchForDataTable(length, page, sortColumn, sortColumnAscDesc, search);

            return result.data;
        }

        public async Task<IEnumerable<sp_SearchUsers>> SearchForDataTableUsersCompany(int length, int page, string sortColumn, string sortColumnAscDesc, string search, int? CompanyId = null)
        {
            var result = await _userService.SearchForDataTableUsersCompany(length, page, sortColumn, sortColumnAscDesc, search, CompanyId);

            return result.data;
        }

        /// <summary>
        /// Tüm firmaları isme göre sıralayan metod
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Company>> GetCompanies()
        {
            var result = await _userService.GetCompanies();

            return result.data;
        }

        public async Task<IEnumerable<Roles>> GetAllRolesByUserId(int UserId)
        {
            var result = await _userService.GetAllRolesByUserId(UserId);

            return result.data;
        }

        public async Task<Company> GetCompanyById(int Id)
        {
            var result = await _userService.GetCompanyById(Id);

            return result.data;
        }

        public async Task<Company> GetCompaniesByTaxNumber(string TaxNumber)
        {
            var result = await _userService.GetCompanyByTaxNumber(TaxNumber);

            return result.data;
        }

        public async Task<Company> InsertCompany(string CompanyName, string TaxOffice, string TaxNumber, string Address, int CityId, int DistrictId, bool IsApproved, string CompanyCode)
        {
            var result = await _userService.InsertCompany(CompanyName, TaxOffice, TaxNumber, Address, CityId, DistrictId, IsApproved, CompanyCode);

            return result.data;
        }

        public async Task<Users> GetUserByUsername(string Username)
        {
            var result = await _userService.GetUserByUsername(Username);

            return result.data;
        }

        public async Task<Users> GetUserByEMail(string EMailAddress)
        {
            var result = await _userService.GetUserByEMail(EMailAddress);

            return result.data;
        }

        public async Task<Users> InsertUser(int CompanyId, string Username, string Password, string EMailAddress, string PhoneNumber, string Name, string Surname, bool IsActive, bool IsApproved)
        {
            var result = await _userService.InsertUser(CompanyId, Username, _utility.GetSHA512Encrypt(Password), EMailAddress, PhoneNumber, Name, Surname, IsActive, IsApproved);

            return result.data;
        }


        /// <summary>
        /// Datatable için verilen firmalar arama fonksiyonu
        /// </summary>
        /// <param name="length"></param>
        /// <param name="page"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortColumnAscDesc"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IEnumerable<sp_SearchCompanies>> CompanySearchForDataTable(int length, int page, string sortColumn, string sortColumnAscDesc, string search)
        {
            var result = await _userService.CompanySearchForDataTable(length, page, sortColumn, sortColumnAscDesc, search);

            return result.data;
        }

        public async Task<bool> UpdateCompany(Company company)
        {
            var result = await _userService.UpdateCompany(company);

            return result.data;
        }

        public async Task<bool> UpdateUser(Users entity)
        {
            var result = await _userService.UpdateUser(entity);

            return result.data;
        }

        public async Task<bool> MakeUserActivePassive(int Id, bool IsActive)
        {
            var result = await _userService.MakeUserActivePassive(Id, IsActive);

            return result.data;
        }

        public async Task<bool> MakeUserApprove(int Id, bool IsApproved)
        {
            var result = await _userService.MakeUserApprove(Id, IsApproved);

            return result.data;
        }

        public async Task<bool> MakeCompanyApprove(int Id, bool IsApproved)
        {
            var result = await _userService.MakeCompanyApprove(Id, IsApproved);

            return result.data;
        }

        public async Task<bool> UpdateRole(Roles entity)
        {
            var result = await _userService.UpdateRole(entity);

            return result.data;
        }

        public async Task<Roles> InsertRole(string Name, string SystemName, bool IsSystemRole)
        {
            var result = await _userService.InsertRole(Name, SystemName, IsSystemRole);

            return result.data;
        }

        public async Task<bool> DeleteRolesPermissionsByRoleId(int RoleId)
        {
            var rolespermissions = await _userService.GetAllRolesPermissionsByRoleId(RoleId);
            var rolesusers = await _userService.GetAllUsersRolesByRoleId(RoleId);
            foreach (var rolespermission in rolespermissions.data)
            {
                await _userService.DeleteRolesPermissionById(RoleId);
            }

            foreach (var rolesuser in rolesusers.data)
            {
                await _userService.DeleteUsersRolesById(RoleId);
            }

            var result = await _userService.DeleteRolesById(RoleId);

            return result.data;
        }

        /// <summary>
        /// Gönderdiğimiz rol sistemde varsa aynen kalıyor. Yoksa ekleniyor...
        /// </summary>
        /// <param name="RoleIds_PermissionIds"></param>
        /// <returns></returns>
        public async Task<bool> AddRoleForRolePermissionUpdate(List<RolesPermissions> RoleIds_PermissionIds)
        {
            int result = 0;

            foreach (var RoleId_PermissionId in RoleIds_PermissionIds)
            {
                var role_permission = await _userService.GetRolesPermission(RoleId_PermissionId.RoleId, RoleId_PermissionId.PermissionId);
                if (role_permission.data == null)
                {
                    if (await _userService.InsertRolesPermissions(RoleId_PermissionId.RoleId, RoleId_PermissionId.PermissionId) != null)
                        result++;
                }
            }

            return result > 0;
        }

        /// <summary>
        /// Rol ve yetki_id ye göre veritabanından rol-id eşleşmesini kaldırıyoruz...
        /// </summary>
        /// <param name="RoleIds_PermissionIds"></param>
        /// <returns></returns>
        public async Task<bool> RemoveRolePermission(List<RolesPermissions> RoleIds_PermissionIds)
        {
            int result = 0;

            foreach (var RoleId_PermissionId in RoleIds_PermissionIds)
            {
                var role_permission = await _userService.GetRolesPermission(RoleId_PermissionId.RoleId, RoleId_PermissionId.PermissionId);
                if (role_permission != null)
                {
                    if (_userService.DeleteRolesPermissionByRolesIdAndPermissionsId(RoleId_PermissionId.RoleId, RoleId_PermissionId.PermissionId).Result.data)
                        result++;
                }
            }

            return result > 0;
        }

        public async Task<bool> EditUserRole(int UserId, string[] RoleIds)
        {
            int result = 0;

            List<int> _RoleIds = new List<int>();
            foreach (var RoleId in RoleIds)
                _RoleIds.Add(int.Parse(RoleId));

            var userRoles = await _userService.GetAllRolesByUserId(UserId);
            foreach (var _RoleId in _RoleIds)
            {
                if (!userRoles.data.Any(x => x.Id == _RoleId))
                {
                    await _userService.InsertUserRole(UserId, _RoleId);
                }
            }

            foreach (var userRole in userRoles.data.Where(x => !x.IsSystemRole))
            {
                if (!_RoleIds.Any(x => x == userRole.Id))
                {
                    if (_userService.DeleteUsersRolesByUserIdAndRoleId(UserId, userRole.Id).Result.data)
                    {
                        result++;
                    }
                }
            }

            return result > 0;
        }

        public async Task<bool> EditUserCustomPermissions(int UserId, string[] PermissionIds)
        {
            int result = 0;

            var role = await _userService.GetSystemRoleByUserId(UserId);
            if (role.data == null && PermissionIds.Length > 0)
            {
                string role_name = string.Format("CustomRole-{0}", UserId);
                role = await _userService.InsertRole(role_name, role_name.Replace("-", ""), true);
                if (role.data != null)
                {
                    await _userService.InsertUserRole(UserId, role.data.Id);
                    foreach (var permissionId in PermissionIds)
                    {
                        int.TryParse(permissionId, out int _permissionId);
                        result += await _userService.InsertRolesPermissions(role.data.Id, _permissionId) != null ? 1 : 0;
                    }
                }
            }
            //Kullanıcının önceden bir özel rolü varmış ama şimdi kaldırılmak isteniyor, özel yetki verilmiyor ise...
            else if (role.data != null && PermissionIds.Length <= 0)
            {
                await _userService.DeleteUsersRolesByUserIdAndRoleId(UserId, role.data.Id);
                return _userService.DeleteRolesById(role.data.Id).Result.data;
            }
            else if (role.data != null && PermissionIds.Length > 0)
            {
                List<RolesPermissions> activeRolePermissions = new List<RolesPermissions>();
                List<RolesPermissions> passiveRolePermissions = new List<RolesPermissions>();
                List<int> new_permission_list = new List<int>();
                foreach (var _permissionId in PermissionIds)
                {
                    new_permission_list.Add(int.Parse(_permissionId));
                }

                foreach (var newPermissionId in new_permission_list)
                {
                    activeRolePermissions.Add(new RolesPermissions { RoleId = role.data.Id, PermissionId = newPermissionId, InsertDate = DateTime.Now });
                }

                var roles_permissions = await _userService.GetAllRolesPermissionsByRoleId(role.data.Id);
                foreach (var role_permission in roles_permissions.data)
                {
                    if (!activeRolePermissions.Any(x => x.PermissionId == role_permission.PermissionId))
                    {
                        passiveRolePermissions.Add(role_permission);
                    }
                }

                result += await AddRoleForRolePermissionUpdate(activeRolePermissions) ? 1 : 0;
                result += await RemoveRolePermission(passiveRolePermissions) ? 1 : 0;
            }

            return result > 0;
        }

        public async Task<IEnumerable<Permissions>> GetSystemRolePermissionsByUserId(int UserId)
        {
            var result = await _userService.GetSystemRolePermissionsByUserId(UserId);

            return result.data;
        }

        public async Task<Users> CreateUserNewPasswordToken(int UserId)
        {
            var user = await _userService.GetUserById(UserId);
            if (user.Status)
            {
                user.data.Token = Guid.NewGuid().ToString();
                user.data.TokenExpireDate = DateTime.Now.AddMinutes(30);
                if (_userService.UpdateUser(user.data).Result.data)
                {
                    return user.data;
                }
            }

            return null;
        }

        public async Task<Tuple<bool, string>> SetDisableUserPasswordToken(string Token, string Password)
        {
            var user = await _userService.GetUserByToken(Token);
            if (user.Status)
            {
                if (user.data != null)
                {
                    if (user.data.TokenExpireDate > DateTime.Now)
                    {
                        if (await CanItBeChangedPassword(user.data.Id, Password))
                        {
                            if (await ChangePassword(user.data.Id, Password))
                            {
                                user = await _userService.GetUserById(user.data.Id);
                                user.data.TokenExpireDate = null;
                                user.data.Token = string.Empty;
                                if (_userService.UpdateUser(user.data).Result.data)
                                {
                                    return new Tuple<bool, string>(true, "Şifre başarıyla değiştirildi.");
                                }
                                else
                                {
                                    return new Tuple<bool, string>(false, "Sifre değiştirilirken hata oluştu!");
                                }
                            }
                            else
                            {
                                return new Tuple<bool, string>(false, "Şifre değiştirilir iken hata ile karşılaşıldı!");
                            }
                        }
                        else
                        {
                            return new Tuple<bool, string>(false, "Yeni şifreniz son 5 şifreniz ile aynı olamaz!");
                        }
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Bağlantının süresi dolmuş durumda!");
                    }
                }
            }

            return new Tuple<bool, string>(false, "Bağlantı geçersiz veya süresi dolmuş!");
        }

        public async Task<Users> SetApprovePhoneEMail(int UserId)
        {
            var user = await _userService.GetUserById(UserId);
            if (user.Status)
            {
                user.data.EMailToken = Guid.NewGuid().ToString().Split('-')[0];
                user.data.EMailTokenExpireDate = DateTime.Now.AddMinutes(30);
                user.data.EMailApproved = false;
                user.data.PhoneToken = Guid.NewGuid().ToString().Split('-')[0];
                user.data.PhoneTokenExpireDate = DateTime.Now.AddMinutes(30);
                user.data.PhoneApproved = false;

                if (_userService.UpdateUser(user.data).Result.Status)
                {
                    return user.data;
                }
            }

            return null;
        }

        public async Task<Users> SetApprovePhone(int UserId)
        {
            var user = await _userService.GetUserById(UserId);
            if (user.Status)
            {
                user.data.PhoneToken = Guid.NewGuid().ToString().Split('-')[0];
                user.data.PhoneTokenExpireDate = DateTime.Now.AddMinutes(30);
                user.data.PhoneApproved = false;

                if (_userService.UpdateUser(user.data).Result.Status)
                {
                    return user.data;
                }
            }

            return null;
        }

        public async Task<Users> SetApproveEMail(int UserId)
        {
            var user = await _userService.GetUserById(UserId);
            if (user.Status)
            {
                user.data.EMailToken = Guid.NewGuid().ToString();
                user.data.EMailTokenExpireDate = DateTime.Now.AddMinutes(30);
                user.data.EMailApproved = false;

                if (_userService.UpdateUser(user.data).Result.Status)
                {
                    return user.data;
                }
            }

            return null;
        }

        public async Task<UsersLoginCode> SetUserLoginCode(int UserId)
        {
            UsersLoginCode entity = new UsersLoginCode();
            entity.InsertDate = DateTime.Now;
            entity.IsActive = true;
            entity.PassiveDate = DateTime.Now.AddMinutes(30);
            entity.Token = Guid.NewGuid().ToString().Split('-')[0].ToUpper();
            entity.UserId = UserId;

            var result = await _userService.SetUserLoginCode(entity);

            return result.data;
        }

        public async Task<bool> ApprovePhoneCodeCheck(int UserId, string Token)
        {
            var user = await _userService.GetUserById(UserId);
            if (user.Status)
            {
                if (user.data.PhoneToken.ToUpper() == Token.ToUpper())
                {
                    user.data.PhoneApproved = true;
                    return _userService.UpdateUser(user.data).Result.Status;
                }
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> ApproveEMailCodeCheck(int UserId, string Token)
        {
            var user = await _userService.GetUserById(UserId);
            if (user.Status)
            {
                if (user.data.EMailToken.ToUpper() == Token.ToUpper())
                {
                    user.data.EMailApproved = true;
                    return _userService.UpdateUser(user.data).Result.Status;
                }
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> ApprovePhoneEMailCodeCheck(int UserId, string PhoneToken, string EMailToken)
        {
            var user = await _userService.GetUserById(UserId);
            if (user.Status)
            {
                if (user.data.EMailToken.ToUpper() == EMailToken.ToUpper() && user.data.PhoneToken.ToUpper() == PhoneToken.ToUpper())
                {
                    user.data.EMailApproved = true;
                    user.data.PhoneApproved = true;
                    return _userService.UpdateUser(user.data).Result.Status;
                }
                else
                    return false;
            }
            return false;
        }

        public async Task<bool> UsersLoginCodeCheck(int UserId, string Token)
        {
            var userLoginCode = await _userService.GetUserLoginCode(UserId, Token);
            if (userLoginCode.data != null)
            {
                userLoginCode.data.IsActive = false;
                userLoginCode.data.PassiveDate = DateTime.Now;
                if (_userService.UpdateUserLoginCode(userLoginCode.data).Result.data)
                    return true;
            }

            return false;
        }
    }
}
