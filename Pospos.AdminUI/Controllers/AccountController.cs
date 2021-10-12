using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Pospos.AdminUI.Helpers;
using Pospos.AdminUI.Models;
using Pospos.Business.Factories;
using Pospos.Core.Common;
using Pospos.Core.Helpers;
using Pospos.Domain.DataTransferObjects;
using Pospos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pospos.AdminUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly UserFactory _userFactory;
        private readonly CommonFactory _commonFactory;
        private readonly IRecaptchaValidator _recaptchaValidator;
        private readonly CacheFactory _cacheFactory;
        private readonly PaymentProcessFactory _paymentProcessFactory;

        public AccountController(UserFactory userFactory, CommonFactory commonFactory, IRecaptchaValidator recaptchaValidator, CacheFactory cacheFactory, IStringLocalizer<AccountController> localizer, PaymentProcessFactory paymentProcessFactory)
        {
            this._userFactory = userFactory;
            this._commonFactory = commonFactory;
            this._recaptchaValidator = recaptchaValidator;
            this._cacheFactory = cacheFactory;
            this._localizer = localizer;
            this._paymentProcessFactory = paymentProcessFactory;
        }

        [Route("login")]
        public IActionResult Login(bool logout = false, string returnUrl = null)
        {
            LoginViewModel model = new LoginViewModel(logout, returnUrl);
            return View(model);
        }

        private string[] whiteList =
        {
            "",
            "/",
            "/payments/list",
            "/payments/bank/list",
            "/payments/canceled-payments",
            "/users",
            "/users/roles",
            "/users/role-permissions",
            "/member-businesses",
            "/member-businesses/create-update",
            "/users/create-update",
            "/login?returnUrl=/payments/list/?token="
        };

        public IActionResult SecurityReturnUrl(string returnUrl)
        {
            string url_token = returnUrl.Replace("/login?returnUrl=/payments/list/?token=", "");
            string url = returnUrl.Replace(url_token, "");

            if (!whiteList.Contains(url.ToLower()))
                return BadRequest();
            else
                return Redirect(returnUrl);
        }

        //Kullanıcı girişi post işlemi burada yürütülüyor. Eğer belirtilen sayıdan fazla deneme var ise kullanıcı belirtilen süre boyunca bloke ediliyor...
        [Route("login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string returnUrl, string email, string password)
        {
            LoginViewModel model = new LoginViewModel();

            int giris_bloke_deneme_adedi = 5;
            int giris_bloke_suresi = 15;
            int giris_denemesi = SessionExtensions.GetInt32(HttpContext.Session, "LoginAttempt");
            if (giris_denemesi == 0)
            {
                SessionExtensions.SetDateTime(HttpContext.Session, "LoginFirstAttemptDateTime", DateTime.Now);
            }

            giris_denemesi++;
            SessionExtensions.SetInt32(HttpContext.Session, "LoginAttempt", giris_denemesi);

            if (giris_denemesi > giris_bloke_deneme_adedi && SessionExtensions.GetDateTime(HttpContext.Session, "LoginFirstAttemptDateTime").AddMinutes(giris_bloke_suresi) > DateTime.Now)
            {
                model.Success = false;
                model.Message = string.Format("Ard arda bir çok kez hatalı giriş denendi. Hesabınız {0} dakikalığına bloke edilmiştir. " +
                    "Lütfen {0} dakika sonra tekrar deneyin.", giris_bloke_suresi);

                return View(model);
            }

            var user = await _userFactory.UserLogin(email, password);
            if (user != null)
            {
                if (!user.IsActive)
                {
                    model.Success = false;
                    model.Message = _localizer["user.passive"];
                    return View(model);
                }

                if (!user.IsApproved)
                {
                    model.Success = false;
                    model.Message = "Kullanıcınız onay aşamasındadır. Lütfen PosPos yöneticisi tarafından gelecek onay e-postasını bekleyiniz.";
                    return View(model);
                }

                bool twowayauthentication = bool.Parse(await _commonFactory.GetSetting("login.twowayauthentication.enable"));
                if (twowayauthentication)
                {
                    var user_code = await _userFactory.SetUserLoginCode(user.Id);
                    if (user_code != null)
                    {
                        if (_commonFactory.SendLoginSms(user.PhoneNumber, user_code.Token).Result.Item1)
                        {
                            SessionExtensions.Set<UsersLoginCodeModel>(HttpContext.Session, "TwoWayAuth", new UsersLoginCodeModel
                            {
                                Id = user_code.Id,
                                UserId = user_code.UserId,
                                InsertDate = user_code.InsertDate,
                                IsActive = user_code.IsActive,
                                PassiveDate = user_code.PassiveDate,
                                Token = user_code.Token,
                                Timeout = 60,
                                NewResendStartDate = DateTime.Now.AddSeconds(60)
                            });

                            if (!string.IsNullOrWhiteSpace(returnUrl))
                                return Redirect("login/two-way-authentication?returnUrl=" + returnUrl);
                            else
                                return Redirect("login/two-way-authentication");
                        }
                        else
                        {
                            model.Success = false;
                            model.Message = "Üzgünüz, sistem tarafından giriş kodunuz gönderilemedi! Lütfen daha sonra tekrar deneyin.";
                            return View(model);
                        }
                    }
                    else
                    {
                        model.Success = false;
                        model.Message = "Üzgünüz, sistem tarafından giriş kodunuz oluşturulamadı! Lütfen daha sonra tekrar deneyin.";
                        return View(model);
                    }
                }
                else
                {
                    LoginSessionData session = new LoginSessionData();

                    if (user.PasswordExpireDate <= DateTime.Now)
                        session.IsActive = false;

                    session.Username = user.Username;
                    session.CreateDate = DateTime.Now;
                    session.NameSurname = string.Format("{0} {1}", user.Name, user.Surname);
                    session.UserId = user.Id;
                    session.CompanyId = user.CompanyId;

                    SessionExtensions.Set<LoginSessionData>(HttpContext.Session, "UserData", session);

                    //Tüm yetkileri cacheliyoruz...
                    await _cacheFactory.SetPermissions();

                    if (!string.IsNullOrWhiteSpace(returnUrl))
                        return SecurityReturnUrl(returnUrl);
                    else
                        return Redirect("/");
                }
            }

            model.Success = false;
            model.Message = "Kullanıcı adı ve/veya şifreniz hatalı!";

            return View(model);
        }

        [Route("login/two-way-authentication/resend")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TwoWayAuthenticationResend()
        {
            var twoWayAuth = SessionExtensions.Get<UsersLoginCodeModel>(HttpContext.Session, "TwoWayAuth");
            if (twoWayAuth == null)
                return Json(new { success = false, message = "Talep süresi dolmuş durumda!" });

            if (twoWayAuth.NewResendStartDate > DateTime.Now)
                return Json(new { success = false, message = "Belirtilen süreden önce tekrar kod talep edemezsiniz!" }); ;

            var user = await _userFactory.GetUserById(twoWayAuth.UserId.GetValueOrDefault(0));
            var user_code = await _userFactory.SetUserLoginCode(twoWayAuth.UserId.GetValueOrDefault(0));
            if (user_code != null)
            {
                if (_commonFactory.SendLoginSms(user.PhoneNumber, user_code.Token).Result.Item1)
                {
                    twoWayAuth.NewResendStartDate = DateTime.Now.AddSeconds(60);
                    SessionExtensions.Set<UsersLoginCodeModel>(HttpContext.Session, "TwoWayAuth", twoWayAuth);

                    return Json(new { success = true, message = "Tekrar gönderim yapıldı." });
                }
                else
                {
                    return Json(new { success = false, message = "Üzgünüz, sistem tarafından giriş kodunuz gönderilemedi! Lütfen daha sonra tekrar deneyin." });
                }
            }
            else
            {
                return Json(new { success = false, message = "Üzgünüz, sistem tarafından giriş kodunuz oluşturulamadı! Lütfen daha sonra tekrar deneyin." });
            }


            return Json(new { success = false, message = "Tekrar gönderim yapılamadı." });
        }

        [Route("login/two-way-authentication")]
        public async Task<IActionResult> TwoWayAuthentication(string returnUrl)
        {
            TwoWayAuthenticationViewModel model = new TwoWayAuthenticationViewModel();

            var twoWayAuth = SessionExtensions.Get<UsersLoginCodeModel>(HttpContext.Session, "TwoWayAuth");
            if (twoWayAuth == null)
                return Redirect("login");

            model.Timeout = twoWayAuth.Timeout;

            return View(model);
        }

        [Route("login/two-way-authentication")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TwoWayAuthentication(string returnUrl, string Code)
        {
            TwoWayAuthenticationViewModel model = new TwoWayAuthenticationViewModel();

            var twoWayAuth = SessionExtensions.Get<UsersLoginCodeModel>(HttpContext.Session, "TwoWayAuth");
            if (twoWayAuth == null)
                return Redirect("login");

            if (await _userFactory.UsersLoginCodeCheck(twoWayAuth.UserId.GetValueOrDefault(0), Code.ToUpper()))
            {
                var user = await _userFactory.GetUserById(twoWayAuth.UserId.GetValueOrDefault(0));

                if (user != null)
                {
                    LoginSessionData session = new LoginSessionData();

                    if (user.PasswordExpireDate <= DateTime.Now)
                        session.IsActive = false;

                    session.CreateDate = DateTime.Now;
                    session.NameSurname = string.Format("{0} {1}", user.Name, user.Surname);
                    session.UserId = user.Id;
                    session.CompanyId = user.CompanyId;

                    SessionExtensions.Set<LoginSessionData>(HttpContext.Session, "UserData", session);

                    //Tüm yetkileri cacheliyoruz...
                    await _cacheFactory.SetPermissions();

                    if (!string.IsNullOrWhiteSpace(returnUrl))
                        return SecurityReturnUrl(returnUrl);
                    else
                        return Redirect("/");
                }
                else
                {
                    model.Success = false;
                    model.Message = "Kullanıcı bulunamadı! Lütfen anasayfaya giderek tekrar deneyin.";
                }
            }
            else
            {
                model.Success = false;
                model.Message = "Giriş kodu hatalı veya geçersiz.";
            }

            return View(model);
        }

        [Route("goto-payment/token={token}")]
        public async Task<IActionResult> GoToPayment(string token)
        {
            var payment = await _paymentProcessFactory.GetByToken(token);
            if (payment != null)
            {
                if (payment.TokenExpireDate.GetValueOrDefault(DateTime.Now.AddDays(-1)) > DateTime.Now)
                {
                    return Redirect(string.Format("/login?returnUrl=%2Fpayments%2Flist%2F?token={0}", payment.Token));
                }
                else
                {
                    return Redirect("/login");
                }
            }
            else
            {
                return Redirect("/login");
            }
        }

        [Route("RecaptchaV3Vverify")]
        [HttpGet]
        //Google captcha'yi burada json ile kontrol ediyoruz...
        public async Task<JsonResult> RecaptchaV3Vverify(string token)
        {
            return new JsonResult(await _recaptchaValidator.IsRecaptchaValid(token));
        }

        [Route("forgot-password")]
        public IActionResult ForgotPassword()
        {
            ForgotPasswordViewModel model = new ForgotPasswordViewModel();

            return View(model);
        }

        [Route("forgot-password")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            ForgotPasswordViewModel model = new ForgotPasswordViewModel();

            var user = await _userFactory.GetUserByEMail(email);
            if (user != null)
            {
                user = await _userFactory.CreateUserNewPasswordToken(user.Id);
                if (user != null)
                {
                    MessageSender eMailSend = new MessageSender();
                    var result = await _commonFactory.SendChangePasswordEMail(user.EMailAddress, string.Format("{0} {1}", user.Name, user.Surname), user.Token);

                    model.Success = result.Item1;
                    model.Message = result.Item2;
                    if (result.Item1)
                        model.Message = "Şifre sıfırlama linkiniz e-posta adresinize gönderilmiştir. Süresi 30 dakikadır.";
                }
                else
                {
                    model.Success = false;
                    model.Message = "Kullanıcı bulunamadı!";
                }
            }
            else
            {
                model.Success = false;
                model.Message = "Kullanıcı bulunamadı!";
            }

            return View(model);
        }

        [Route("reset-password/token={token}")]
        public async Task<IActionResult> ResetPassword(string token)
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.Token = token;

            int giris_bloke_deneme_adedi = 5;
            int giris_bloke_suresi = 15;
            int giris_denemesi = SessionExtensions.GetInt32(HttpContext.Session, "LoginAttempt");
            if (giris_denemesi == 0)
            {
                SessionExtensions.SetDateTime(HttpContext.Session, "LoginFirstAttemptDateTime", DateTime.Now);
            }

            giris_denemesi++;
            SessionExtensions.SetInt32(HttpContext.Session, "LoginAttempt", giris_denemesi);

            if (giris_denemesi > giris_bloke_deneme_adedi && SessionExtensions.GetDateTime(HttpContext.Session, "LoginFirstAttemptDateTime").AddMinutes(giris_bloke_suresi) > DateTime.Now)
            {
                model.Success = false;
                model.Message = string.Format("Ard arda bir çok kez hatalı token denendi. Hesabınız {0} dakikalığına bloke edilmiştir. " +
                    "Lütfen {0} dakika sonra tekrar deneyin.", giris_bloke_suresi);

                return View(model);
            }

            var result = await _userFactory.GetUserByToken(token);
            if (result != null)
            {
                if (result.TokenExpireDate.GetValueOrDefault(DateTime.Now.AddDays(-1)) < DateTime.Now)
                {
                    model.Success = false;
                    model.Message = "Üzgünüz, belirtilen bilet geçersiz veya süresi dolmuş!";
                }
                else
                {
                    model.SuccessToken = true;
                }
            }
            else
            {
                model.Success = false;
                model.Message = "Üzgünüz, belirtilen bilet geçersiz veya süresi dolmuş!";
            }

            return View(model);
        }

        [Route("reset-password")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string Token, string Password, string PasswordAgain)
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();

            var user = await _userFactory.GetUserByToken(Token);
            if (user != null)
            {
                if (user.TokenExpireDate.GetValueOrDefault(DateTime.Now.AddDays(-1)) < DateTime.Now)
                {
                    model.Success = false;
                    model.Message = "Üzgünüz, belirtilen bilet geçerli veya süresi dolmuş!";
                }
            }
            else
            {
                model.Success = false;
                model.Message = "Üzgünüz, belirtilen bilet geçerli veya süresi dolmuş!";
            }

            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(PasswordAgain))
            {
                model.Success = false;
                model.Message = "Şifre ve/veya şifre tekrarı boş bırakılamaz!";

                return View(model);
            }

            if (Password != PasswordAgain)
            {
                model.Success = false;
                model.Message = "Şifre ve şifre aynı olmalıdır!";

                return View(model);
            }

            if (!SecurityHelper.PasswordHighSecurityControl(Password))
            {
                model.Success = false;
                model.Message = "Şifreniz en az 8 karakterden oluşmalı, en az bir rakam, bir büyük, bir küçük harf ve bir özel karakter (_-*+... gibi) içermelidir.";
                return View(model);
            }

            var result = await _userFactory.SetDisableUserPasswordToken(Token, Password);

            model.Success = result.Item1;
            model.Message = result.Item2;

            return View(model);
        }

        [SessionExpire]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword()
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();

            return View(model);
        }

        [SessionExpire]
        [Route("change-password")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string nowpassword, string password, string passwordAgain)
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();

            if (string.IsNullOrWhiteSpace(nowpassword) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordAgain))
            {
                model.Message = "Tüm alanlar doldurulmalıdır! Lütfen kontrol ederek tekrar deneyin.";
                return View(model);
            }

            if (password != passwordAgain)
            {
                model.Message = "Şifre ve Şifre Tekrarı uyuşmamaktadır! Lütfen kontrol ederek tekrar deneyin.";
                return View(model);
            }

            //Session'dan kullanıcı bilgisini çekiyoruz...
            var session = SessionExtensions.Get<LoginSessionData>(HttpContext.Session, "UserData");

            if (!await _userFactory.CheckUserPassword(session.UserId, nowpassword))
            {
                model.Message = "Mevcut şifrenizi hatalı girdiniz! Lütfen kontrol ederek tekrar deneyin.";
                return View(model);
            }

            //Şifresi son şifreleri ile aynı mı kontrol ediyoruz...
            bool canItBeChangedPassword = await _userFactory.CanItBeChangedPassword(session.UserId, password);
            if (!canItBeChangedPassword)
            {
                model.Message = "Son 5 şifrenizden birini kullanamazsınız. Lütfen değiştirerek tekrar deneyin.";
                return View(model);
            }

            if (await _userFactory.ChangePassword(session.UserId, password))
            {
                model.Success = true;
                model.Message = "Şifre başarılı bir şekilde değiştirildi. Yönlendiriliyorsunuz, lütfen bekleyin.";
            }

            session.IsActive = true;
            SessionExtensions.Set<LoginSessionData>(HttpContext.Session, "UserData", session);

            return View(model);
        }

        [SessionExpire]
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            if (SessionExtensions.SessionClear(HttpContext.Session, "UserData"))
            {
                return Redirect("/login?logout=true");
            }
            else
            {
                return View();
            }
        }

        [SessionExpire]
        [Route("users/roles")]
        public async Task<IActionResult> Roles()
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PageRoleManagement))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            return View();
        }

        [SessionExpire]
        [Route("users/roles-list")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RolesList()
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PageRoleManagement))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            var kullanicilar = _userFactory.GetAllRoles().Result.Where(x => !x.IsSystemRole);

            return Json(new { data = kullanicilar, draw = Request.Form["draw"] });
        }

        [SessionExpire]
        [Route("users/roles/create-update")]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _RolesCreateUpdate(int? Id)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.CreateRole)
                && !await _cacheFactory.PermissionControl(PanelPermissions.EditRole))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            CreateUpdateRolesModalPopupViewModel model = new CreateUpdateRolesModalPopupViewModel();

            if (Id.GetValueOrDefault(0) > 0)
            {
                var roles = await _userFactory.GetAllRoles();
                if (roles.Count() > 0)
                {
                    var role = roles.Where(x => x.Id == Id).FirstOrDefault();
                    if (role != null)
                    {
                        model.Id = role.Id;
                        model.Name = role.Name;
                        model.SystemName = role.SystemName;
                    }
                }
            }

            return PartialView(model);
        }

        [SessionExpire]
        [Route("users/roles/create-update")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _RolesCreateUpdate(int? Id, string Name, string SystemName, bool IsSystemRole)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.CreateRole)
                && !await _cacheFactory.PermissionControl(PanelPermissions.EditRole))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            CreateUpdateRolesModalPopupViewModel model = new CreateUpdateRolesModalPopupViewModel();

            //update işlemi ise buradan...
            if (Id.GetValueOrDefault(0) > 0)
            {
                var roles = await _userFactory.GetAllRoles();
                if (roles.Count() > 0)
                {
                    var role = roles.Where(x => x.Id == Id).FirstOrDefault();
                    if (role != null)
                    {
                        role.Name = Name;
                        role.SystemName = SystemName;
                        role.IsSystemRole = IsSystemRole;

                        if (await _userFactory.UpdateRole(role))
                            return Json(new { success = true, message = "Kayıt işlemi başarıyla güncellendi." });
                    }
                }
            }
            else
            {
                var role = await _userFactory.InsertRole(Name, SystemName, IsSystemRole);
                if (role != null)
                    return Json(new { success = true, message = "Kayıt işlemi başarıyla tamamlandı." });
            }

            return Json(new { success = false, message = "İşlem sırasında bir hata oluştu!" });
        }

        [SessionExpire]
        [Route("users/role-permissions")]
        public async Task<IActionResult> RolePermissions(int? editRoleId)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.ChangeRolePermissions))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            PermissionsViewModel model = new PermissionsViewModel();
            model.Permissions = await _userFactory.GetAllPermissions();
            model.Roles = await _userFactory.GetRoles();
            model.RolesPermissions = await _userFactory.GetAllRolesPermissions();

            model.editRoleId = editRoleId.GetValueOrDefault(0);

            if (editRoleId.GetValueOrDefault(0) > 0)
            {
                model.Roles = model.Roles.Where(x => x.Id == editRoleId.Value);
            }

            return View(model);
        }

        [SessionExpire]
        [Route("users/role-permissions")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RolePermissions(string checkedbox_value, string uncheckedbox_value, int? editId = 0)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.ChangeRolePermissions))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            if (!string.IsNullOrWhiteSpace(checkedbox_value) && !string.IsNullOrWhiteSpace(uncheckedbox_value))
            {
                string[] aktifler = checkedbox_value.TrimEnd(',').Split(',');
                List<RolesPermissions> aktifler_listesi = new List<RolesPermissions>();
                foreach (var name in aktifler)
                {
                    int.TryParse(name.Split('-')[1].Split('_')[0], out int key);
                    int.TryParse(name.Split('-')[1].Split('_')[1], out int value);

                    aktifler_listesi.Add(new RolesPermissions { RoleId = key, PermissionId = value });
                }
                //_accountService.RolYetkiEkle(aktifler_listesi);
                await _userFactory.AddRoleForRolePermissionUpdate(aktifler_listesi);

                string[] pasifler = uncheckedbox_value.TrimEnd(',').Split(',');
                List<RolesPermissions> pasifler_listesi = new List<RolesPermissions>();
                foreach (var name in pasifler)
                {
                    int.TryParse(name.Split('-')[1].Split('_')[0], out int key);
                    int.TryParse(name.Split('-')[1].Split('_')[1], out int value);

                    pasifler_listesi.Add(new RolesPermissions { RoleId = key, PermissionId = value });
                }
                //_accountService.RolYetkiSil(pasifler_listesi);
                await _userFactory.RemoveRolePermission(pasifler_listesi);

                return Json(new { success = true, message = "Yetkiler başarıyla düzenlendi." });
            }

            return Json(new { success = false, message = "Yetkiler sisteme işlenemedi!" });
        }

        [SessionExpire]
        [Route("customers")]
        public async Task<IActionResult> Users()
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PageUserManagement))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            return View();
        }

        [SessionExpire]
        [Route("customers")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsersList()
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PageUserManagement))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            int draw = Convert.ToInt32(Request.Form["draw"]);// etkin sayfa numarası
            int start = Convert.ToInt32(Request.Form["start"]);//listenen ilk kayıtın  index numarası
            int length = Convert.ToInt32(Request.Form["length"]);//sayfadaki toplam listelenecek kayit sayısı
            string search = Request.Form["search[value]"];//arama

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();

            int page = (start / length) + 1;

            IEnumerable<sp_SearchUsers> kullanicilar;
            if (await _cacheFactory.PermissionControl(PanelPermissions.ProcessingForAllCompany))
            {
                kullanicilar = await _userFactory.SearchForDataTableUsersCompany(length, page, sortColumn, sortColumnAscDesc, search);
            }
            else
            {
                var loginSessionData = SessionExtensions.Get<LoginSessionData>(HttpContext.Session, "UserData");
                kullanicilar = await _userFactory.SearchForDataTableUsersCompany(length, page, sortColumn, sortColumnAscDesc, search, loginSessionData.CompanyId);
            }
            
            int totalRows = 0;
            totalRows = kullanicilar.Count() <= 0 ? 0 : kullanicilar.FirstOrDefault().TotalRowCount;

            return Json(new { data = kullanicilar, draw = Request.Form["draw"], recordsTotal = totalRows, recordsFiltered = totalRows });
        }

        [SessionExpire]
        [Route("companies")]
        public async Task<IActionResult> Companies()
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PageMemberBussinesManagement))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            return View();
        }

        [Route("register")]
        public async Task<IActionResult> Register()
        {
            RegisterViewModel model = new RegisterViewModel();

            model.Cities = await _commonFactory.GetCities();

            return View(model);
        }

        [Route("register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string CompanyName, string Name, string Surname, string EMail, string PhoneNumber, int CityId, int DistrictId, string Address, string TaxOffice, string TaxNumber, bool chkApprove)
        {
            RegisterViewModel model = new RegisterViewModel();
            model.Cities = await _commonFactory.GetCities();

            Company company = await _userFactory.GetCompaniesByTaxNumber(TaxNumber);
            Users user = await _userFactory.GetUserByEMail(EMail);

            if (company == null)
            {
                if (user == null)
                {
                    company = await _userFactory.InsertCompany(CompanyName, TaxOffice, TaxNumber, Address, CityId, DistrictId, false, string.Empty);
                    if (company != null)
                    {
                        PhoneNumber = PhoneNumber.Replace(" ", "")
                                                .Replace("-", "")
                                                .Replace("(", "")
                                                .Replace(")", "");

                        user = await _userFactory.InsertUser(company.Id, EMail, string.Empty, EMail, PhoneNumber, Name, Surname, true, false);
                        if (user != null)
                        {
                            user = await _userFactory.SetApprovePhoneEMail(user.Id);

                            if (user != null)
                            {
                                bool.TryParse(await _commonFactory.GetSetting("login.register.approvecontact.smsenable"), out bool smsenable);
                                bool.TryParse(await _commonFactory.GetSetting("login.register.approvecontact.emailenable"), out bool emailenable);

                                if (emailenable)
                                    await _commonFactory.SendWaitingApproveEMail(user.EMailAddress, string.Format("{0} {1}", user.Name, user.Surname), user.EMailToken);
                                if (smsenable)
                                    await _commonFactory.SendWaitingApproveSms(user.PhoneNumber, user.PhoneToken);

                                SessionExtensions.Set<string>(HttpContext.Session, "ApproveUserId", user.Id.ToString());
                                return Redirect("approve-contacts/" + user.Id);
                            }

                            //model.Success = true;
                            //model.Message = "Kaydınız başarıyla alındı. Giriş yapabilmeniz için en kısa sürede PosPos Yöneticisi tarafından gelecek onay e-postası beklemelisiniz. Şifreniz onay e-postası ile birlikte gönderilecektir.";
                        }
                        else
                        {
                            model.Success = false;
                            model.Message = "Firma kaydı oluşturuldu. Ancak kullanıcı kaydı oluşturulamadı.";
                        }
                    }
                    else
                    {
                        model.Success = false;
                        model.Message = "Firma kaydı oluşturulurken hata oluştu.";
                    }

                }
                else
                {
                    model.Success = false;
                    model.Message = "Belirtilen kullanıcı sistemde zaten kayıtlı. Yeni üye kaydı için lütfen firma yöneticinizle irtibata geçin.";
                }
            }
            else
            {
                model.Success = false;
                model.Message = "Belirtilen firma sistemde zaten kayıtlı. Yeni üye kaydı için lütfen firma yöneticinizle irtibata geçin.";
            }

            return View(model);
        }

        [Route("approve-contacts/{Id}")]
        public async Task<IActionResult> ApproveContacts(int Id)
        {
            ApproveContactsViewModel model = new ApproveContactsViewModel();

            int.TryParse(SessionExtensions.Get<string>(HttpContext.Session, "ApproveUserId"), out int UserId);
            if (UserId <= 0 || UserId != Id)
            {
                return Redirect("/login");
            }

            bool.TryParse(await _commonFactory.GetSetting("login.register.approvecontact.smsenable"), out bool smsenable);
            bool.TryParse(await _commonFactory.GetSetting("login.register.approvecontact.emailenable"), out bool emailenable);

            model.smsenable = smsenable;
            model.emailenable = emailenable;

            return View(model);
        }

        [Route("approve-contacts")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveContacts(string SMSToken, string EMailToken)
        {
            ApproveContactsViewModel model = new ApproveContactsViewModel();

            int.TryParse(SessionExtensions.Get<string>(HttpContext.Session, "ApproveUserId"), out int UserId);
            if (UserId > 0)
            {
                bool.TryParse(await _commonFactory.GetSetting("login.register.approvecontact.smsenable"), out bool smsenable);
                bool.TryParse(await _commonFactory.GetSetting("login.register.approvecontact.emailenable"), out bool emailenable);

                if (smsenable && string.IsNullOrWhiteSpace(SMSToken))
                {
                    model.Message = "SMS onay kodu girilmesi gerekmektedir!";
                }
                else if (emailenable && string.IsNullOrWhiteSpace(EMailToken))
                {
                    model.Message = "E-Posta onay kodu girilmesi gerekmektedir!";
                }
                else if (smsenable && emailenable)
                {
                    model.Success = await _userFactory.ApprovePhoneEMailCodeCheck(UserId, SMSToken, EMailToken);
                    model.Message = model.Success ? "E-Posta ve SMS onayınız başarıyla alındı. Yönetici onayından sonra e-posta ile bilgilendirileceksiniz." : "E-Posta  ve/veya SMS onayı sırasında hata ile karşılaşıldı!";
                    SessionExtensions.SessionClear(HttpContext.Session, "ApproveUserId");
                }
                else if (smsenable)
                {
                    model.Success = await _userFactory.ApprovePhoneCodeCheck(UserId, SMSToken);
                    model.Message = model.Success ? "SMS onayınız başarıyla alındı. Yönetici onayından sonra e-posta ile bilgilendirileceksiniz." : "SMS onayı sırasında hata ile karşılaşıldı!";
                    SessionExtensions.SessionClear(HttpContext.Session, "ApproveUserId");
                }
                else if (emailenable)
                {
                    model.Success = await _userFactory.ApproveEMailCodeCheck(UserId, EMailToken);
                    model.Message = model.Success ? "E-Posta onayınız başarıyla alındı. Yönetici onayından sonra e-posta ile bilgilendirileceksiniz." : "E-Posta onayı sırasında hata ile karşılaşıldı!";
                    SessionExtensions.SessionClear(HttpContext.Session, "ApproveUserId");
                }
                else
                    model.Message = "SMS ve/veya E-posta onay kodunuz hatalı!";
            }
            else
            {
                return Redirect("/login");
            }

            return View(model);
        }

        [SessionExpire]
        [Route("users/create-update")]
        public async Task<IActionResult> CreateOrUpdate(int? editId)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.CreateUser)
                && !await _cacheFactory.PermissionControl(PanelPermissions.EditUser))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            CreateUpdateUserViewModel model = new CreateUpdateUserViewModel();
            model.Companies = await _userFactory.GetCompanies();
            model.Roles = await _userFactory.GetRoles();
            model.Permissions = await _userFactory.GetAllPermissions();

            if (editId.GetValueOrDefault(0) > 0)
            {
                var user = await _userFactory.GetUserById(editId.Value);
                if (user != null)
                {
                    LoginSessionData session = SessionExtensions.Get<LoginSessionData>(HttpContext.Session, "UserData");
                    if (user.CompanyId != session.CompanyId && !await _cacheFactory.PermissionControl(PanelPermissions.ProcessingForAllCompany))
                        return RedirectToAction("UnAuthorizedAccess", "Account");

                    model = await GetUserToModel(user, model);
                    model.SelectedPermissions = await _userFactory.GetSystemRolePermissionsByUserId(editId.Value);
                }
            }

            return View(model);
        }

        [SessionExpire]
        [Route("users/create-update")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrUpdate(int? editId, int CompanyId, string Username, string EMailAddress, string Password, string PasswordAgain, string Name, string Surname, string PhoneNumber, string[] roles, string[] permissions)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.CreateUser)
                && !await _cacheFactory.PermissionControl(PanelPermissions.EditUser))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            CreateUpdateUserViewModel model = new CreateUpdateUserViewModel();
            model.Companies = await _userFactory.GetCompanies();
            model.Roles = await _userFactory.GetRoles();
            model.Permissions = await _userFactory.GetAllPermissions();

            PhoneNumber = string.IsNullOrWhiteSpace(PhoneNumber) ? string.Empty : PhoneNumber;
            EMailAddress = string.IsNullOrWhiteSpace(EMailAddress) ? string.Empty : EMailAddress;

            bool IsApproved = HttpContext.Request.Form["IsApproved"].ToString().ToLower() == "on";
            bool IsActive = HttpContext.Request.Form["IsActive"].ToString().ToLower() == "on";

            //Hata oluşup, input'ların tekrar doldurulması için...
            Users _user = new Users
            {
                CompanyId = CompanyId,
                EMailAddress = EMailAddress,
                IsActive = IsActive,
                IsApproved = IsApproved,
                Name = Name,
                PhoneNumber = PhoneNumber,
                Password = Password,
                Surname = Surname,
                Username = Username
            };

            //Ekleme çalışıyorsa o zaman şifre kontrolü yapıyor. Yoksa şifreye müdahale edilemiyor zaten...
            if (editId.GetValueOrDefault(0) <= 0)
            {
                if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(PasswordAgain))
                {
                    model = await GetUserToModel(_user, model);
                    model.Message = "Şifre ve/veya şifre tekrarı boş olamaz!";
                    return View(model);
                }

                if (Password != PasswordAgain)
                {
                    model = await GetUserToModel(_user, model);
                    model.Message = "Şifre ve şifre tekrarı uyuşmamaktadır!";
                    return View(model);
                }
            }

            var user = await _userFactory.GetUserByUsername(Username);
            if (editId.GetValueOrDefault(0) > 0)
            {
                if (user != null && user.Id != editId.Value)
                {
                    model.Message = "Kullanıcı adı başka bir kullanıcı tarafından kullanılmaktadır!";
                    return View(model);
                }

                user = await _userFactory.GetUserById(editId.Value);
                if (user != null)
                {
                    user.Name = Name;
                    user.Surname = Surname;
                    user.CompanyId = CompanyId;
                    user.Username = Username;
                    user.EMailAddress = EMailAddress;
                    user.PhoneNumber = PhoneNumber
                            .Replace("(", "")
                            .Replace(")", "")
                            .Replace(" ", "")
                            .Replace("-", "");
                    user.IsActive = IsActive;
                    user.IsApproved = IsApproved;
                    if (await _userFactory.UpdateUser(user))
                    {
                        await _userFactory.EditUserRole(user.Id, roles);
                        await _userFactory.EditUserCustomPermissions(user.Id, permissions);

                        model = await GetUserToModel(user, model);
                        model.SelectedPermissions = await _userFactory.GetSystemRolePermissionsByUserId(user.Id);
                        model.Success = true;
                        model.Message = "Kayıt başarıyla güncellendi.";
                    }
                    else
                    {
                        model = await GetUserToModel(_user, model);
                        model.Message = "Kayıt güncellenirken hata ile karşılaşıldı!";
                    }
                }
                else
                {
                    model = await GetUserToModel(_user, model);
                    model.Message = "Kullanıcı bulunamadı!";
                }
            }
            else
            {
                if (user == null)
                {
                    user = await _userFactory.GetUserByEMail(EMailAddress);
                    if (user == null)
                    {
                        PhoneNumber = PhoneNumber
                            .Replace("(", "")
                            .Replace(")", "")
                            .Replace(" ", "")
                            .Replace("-", "");

                        user = await _userFactory.InsertUser(CompanyId, Username, Password, EMailAddress, PhoneNumber, Name, Surname, IsActive, IsApproved);
                        if (user != null)
                        {
                            await _userFactory.EditUserRole(user.Id, roles);

                            if (permissions.Length > 0)
                                await _userFactory.EditUserCustomPermissions(user.Id, permissions);

                            model.SelectedPermissions = await _userFactory.GetSystemRolePermissionsByUserId(user.Id);

                            model = await GetUserToModel(user, model);
                            model.Success = true;
                            model.Message = "Kayıt başarıyla oluşturuldu.";
                        }
                        else
                        {
                            model = await GetUserToModel(_user, model);
                            model.Message = "Kullanıcı oluşturulur iken hata ile karşılaşıldı!";
                        }
                    }
                    else
                    {
                        model = await GetUserToModel(_user, model);
                        model.Message = "E-Posta adresi zaten kullanımda!";
                    }
                }
                else
                {
                    model = await GetUserToModel(_user, model);
                    model.Message = "Kullanıcı adı zaten kullanımda!";
                }
            }

            return View(model);
        }

        [SessionExpire]
        [Route("users/active-passive-user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivePassiveUser(int Id, bool IsActive)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.ApproveUser))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            if (await _userFactory.MakeUserActivePassive(Id, IsActive))
                return Json(new { success = true });

            return Json(new { success = false });
        }

        [SessionExpire]
        [Route("users/approve-user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveUser(int Id, bool IsApproved)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.ApproveUser))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            if (await _userFactory.MakeUserApprove(Id, IsApproved))
            {
                if (IsApproved)
                {
                    var user = await _userFactory.GetUserById(Id);
                    if (user != null)
                    {
                        string password = Guid.NewGuid().ToString().Split('-')[0];
                        await _userFactory.ChangePassword(Id, password);
                        await _commonFactory.SendApproveUserPasswordEMail(user.EMailAddress, string.Format("{0} {1}", user.Name, user.Surname), password);
                        await _commonFactory.SendApproveUserPasswordSms(user.PhoneNumber, password);
                    }
                }

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [SessionExpire]
        [Route("change-password-from-admin")]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _ChangePassword(int Id)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.ChangeUserPassword))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            ChangePasswordModalPopupViewModel model = new ChangePasswordModalPopupViewModel();

            var user = await _userFactory.GetUserById(Id);
            if (user != null)
            {
                model.Id = user.Id;
                model.Username = user.Username;
            }

            return PartialView(model);
        }

        [SessionExpire]
        [Route("change-password-from-admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _ChangePassword(int Id, string Password, string PasswordAgain)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.ChangeUserPassword))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(PasswordAgain))
            {
                return Json(new { success = false, message = "Şifre ve/veya şifre tekrarı boş bırakılamaz!" });
            }

            if (Password != PasswordAgain)
            {
                return Json(new { success = false, message = "Şifre ve şifre tekrarı uyuşmuyor!" });
            }

            if (!SecurityHelper.PasswordHighSecurityControl(Password))
            {
                return Json(new { success = false, message = "Şifre en az 8 karakterden oluşmalı, en az bir rakam, bir büyük, bir küçük harf ve bir özel karakter (_-*+... gibi) içermelidir." });
            }

            if (!await _userFactory.ChangePassword(Id, Password))
            {
                return Json(new { success = false, message = "Şifre değiştirilir iken bir hata ile karşılaşıldı!" });
            }

            return Json(new { success = true, message = string.Empty });
        }

        [SessionExpire]
        [Route("users/roles/delete-role")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(int Id)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.DeleteRole))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            if (Id > 0)
            {
                if (await _userFactory.DeleteRolesPermissionsByRoleId(Id))
                    return Json(new { success = true, message = "Rol ve ilişkileri başarıyla silindi." });
                else
                    return Json(new { success = false, message = "Üzgünüz, silme işlemi sırasında hata ile karşılaşıldı!" });
            }

            return Json(new { success = false, message = "Üzgünüz, silme işlemi sırasında hata ile karşılaşıldı!" });
        }

        [SessionExpire]
        [Route("UnAuthorizedAccess")]
        public async Task<IActionResult> UnAuthorizedAccess()
        {
            return View();
        }

        private async Task<CreateUpdateUserViewModel> GetUserToModel(Users user, CreateUpdateUserViewModel model)
        {
            model.CompanyId = user.CompanyId;
            model.EMailAddress = user.EMailAddress;
            model.Id = user.Id;
            model.InsertDate = user.InsertDate;
            model.IsActive = user.IsActive;
            model.IsApproved = user.IsApproved;
            model.Name = user.Name;
            model.PasswordExpireDate = user.PasswordExpireDate;
            model.PhoneNumber = user.PhoneNumber;
            model.Surname = user.Surname;
            model.Username = user.Username;
            model.Password = user.Password;
            model.Roles = await _userFactory.GetRoles();

            if (user.Id > 0)
                model.SelectedRoles = await _userFactory.GetAllRolesByUserId(user.Id);

            return model;
        }
    }
}
