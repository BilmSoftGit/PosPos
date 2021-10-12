using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Pospos.AdminUI.Helpers;
using Pospos.AdminUI.Models;
using Pospos.Business.Factories;
using Pospos.Core.Common;
using Pospos.Core.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pospos.AdminUI.Controllers
{
    [SessionExpire]
    public class PaymentController : Controller
    {
        private readonly IHostingEnvironment _environment;

        private readonly PaymentProcessFactory _paymentProcessFactory;
        private readonly PaymentProcessCancelFactory _paymentProcessCancelFactory;
        private readonly CacheFactory _cacheFactory;
        private readonly BankFactory _bankFactory;
        private readonly PaymentTypeFactory _paymentTypeFactory;
        private readonly PosFactory _posFactory;
        private readonly StationFactory _stationFactory;
        private readonly CommonFactory _commonFactory;
        private readonly UserFactory _userFactory;

        public PaymentController(PaymentProcessFactory paymentProcessFactory, CacheFactory cacheFactory, BankFactory bankFactory, PaymentTypeFactory paymentTypeFactory, PosFactory posFactory, StationFactory stationFactory, PaymentProcessCancelFactory paymentProcessCancelFactory, CommonFactory commonFactory, UserFactory userFactory, IHostingEnvironment environment)
        {
            this._paymentProcessFactory = paymentProcessFactory;
            this._cacheFactory = cacheFactory;
            this._bankFactory = bankFactory;
            this._paymentTypeFactory = paymentTypeFactory;
            this._posFactory = posFactory;
            this._stationFactory = stationFactory;
            this._paymentProcessCancelFactory = paymentProcessCancelFactory;
            this._commonFactory = commonFactory;
            this._userFactory = userFactory;
            this._environment = environment;
        }

        [Route("payments/successful-payments")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [Route("payments/list")]
        public async Task<IActionResult> List()
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PagePayments))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            PendingPaymentProcessViewModel model = new PendingPaymentProcessViewModel();
            model.Banks = await _bankFactory.GetAll();
            model.PaymentTypes = await _paymentTypeFactory.GetAll();
            model.Poses = await _posFactory.GetAll();

            return View(model);
        }

        [Route("payments/list")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> List(string Username, string CardHolder, string CardNumber, string Token, int? BankId, int? PosId)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PagePayments))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            int _BankId;
            int _PosId;

            Username = Request.Query["Username"].ToString();
            CardHolder = Request.Query["CardHolder"].ToString();
            CardNumber = Request.Query["CardNumber"].ToString();
            Token = Request.Query["Token"].ToString();
            int.TryParse(Request.Query["BankId"].ToString(), out _BankId);
            int.TryParse(Request.Query["PosId"].ToString(), out _PosId);

            if (_BankId > 0)
                BankId = _BankId;

            if (_PosId > 0)
                PosId = _PosId;

            int draw = Convert.ToInt32(Request.Form["draw"]);// etkin sayfa numarası
            int start = Convert.ToInt32(Request.Form["start"]);//listenen ilk kayıtın  index numarası
            int length = Convert.ToInt32(Request.Form["length"]);//sayfadaki toplam listelenecek kayit sayısı
            string search = Request.Form["search[value]"];//arama

            if (!string.IsNullOrWhiteSpace(Token))
                search = Token;

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();

            int page = (start / length) + 1;

            IEnumerable<Domain.DataTransferObjects.sp_SearchPaymentProcess> payments;

            if (await _cacheFactory.PermissionControl(PanelPermissions.ProcessingForAllCompany))
            {
                payments = await _paymentProcessFactory.SearchForDataTablePaymentProcess(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, BankId, 1, PosId);
            }
            else
            {
                var loginSessionData = SessionExtensions.Get<LoginSessionData>(HttpContext.Session, "UserData");
                payments = await _paymentProcessFactory.SearchForDataTablePaymentProcess(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, BankId, 1, PosId, loginSessionData.CompanyId);
            }

            int totalRows = 0;
            totalRows = payments.Count() <= 0 ? 0 : payments.FirstOrDefault().TotalRowCount;

            return Json(new { data = payments, draw = Request.Form["draw"], recordsTotal = totalRows, recordsFiltered = totalRows });
        }

        [Route("payments/excel/list")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListExcel(string Username, string CardHolder, string CardNumber, string Token, int? BankId, int? PosId)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PagePayments))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            int _BankId;
            int _PosId;

            Username = Request.Query["Username"].ToString();
            CardHolder = Request.Query["CardHolder"].ToString();
            CardNumber = Request.Query["CardNumber"].ToString();
            Token = Request.Query["Token"].ToString();
            int.TryParse(Request.Query["BankId"].ToString(), out _BankId);
            int.TryParse(Request.Query["PosId"].ToString(), out _PosId);

            if (_BankId > 0)
                BankId = _BankId;

            if (_PosId > 0)
                PosId = _PosId;

            IEnumerable<Domain.DataTransferObjects.sp_SearchPaymentProcess> payments;

            if (await _cacheFactory.PermissionControl(PanelPermissions.ProcessingForAllCompany))
            {
                payments = await _paymentProcessFactory.SearchForDataTablePaymentProcessForExcel(0, 0, "Id", "asc", "", Username, CardHolder, CardNumber, BankId, 1, PosId);
            }
            else
            {
                var loginSessionData = SessionExtensions.Get<LoginSessionData>(HttpContext.Session, "UserData");
                payments = await _paymentProcessFactory.SearchForDataTablePaymentProcessForExcel(0, 0, "Id", "asc", "", Username, CardHolder, CardNumber, BankId, 1, PosId, loginSessionData.CompanyId);
            }

            List<string[]> key = new List<string[]>()
            {
                 new string[] {
                     "Id",
                     "Firma Adı",
                     "Kullanıcı Adı",
                     "Ödeme Tipi",
                     "Banka Adı",
                     "Toplam Tutar",
                     "Tarih",
                     "Onay Durumu",
                     "Taksit Sayısı",
                     "Kart Sahibi",
                     "Kart Numarası",
                     "AuthCode",
                     "Kullanıcı Ip",
                     "Durum"
                 }
            };

            List<string[]> value = new List<string[]>();

            foreach (var p in payments)
            {
                value.Add(new string[]
                {
                    p.Id.ToString(),
                    p.StationName,
                    p.UserName,
                    p.PaymentTypeName,
                    p.BankName,
                    p.TotalAmount,
                    p.TransDate.GetValueOrDefault(DateTime.Now).ToString("dd/MM/yyyy"),
                    p.IsApproved?"Onaylı":"Onaysız",
                    p.InstalmentCount,
                    p.CardName,
                    p.CardHolder,
                    p.AuthCode,
                    p.ClientIp,
                    p.CancellationLevel.GetValueOrDefault(1).ToString()
                });
            }

            string localPath = @"\Reports\Payments\";
            if (!Directory.Exists(_environment.WebRootPath + localPath))
                Directory.CreateDirectory(_environment.WebRootPath + localPath);

            Dictionary<List<string[]>, List<string[]>> keyValuePairs = new Dictionary<List<string[]>, List<string[]>>();
            keyValuePairs.Add(key, value);

            string fileAddress = _environment.WebRootPath + localPath + string.Format(@"Payments-Report-{0}.xlsx", DateTime.Now.ToString("ddMMyyyy-HHmmss"));
            ExportExcel.ExportExcelSave(keyValuePairs, fileAddress);

            string fileName = Path.GetFileName(fileAddress);

            IFileProvider provider = new PhysicalFileProvider(_environment.WebRootPath + localPath);
            IFileInfo fileInfo = provider.GetFileInfo(fileName);
            var readStream = fileInfo.CreateReadStream();
            var mimeType = "application/vnd.ms-excel";
            return File(readStream, mimeType, fileName);
        }

        [Route("payments/list/success")]
        public async Task<IActionResult> SuccessList()
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PageSuccessPayments))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            PendingPaymentProcessViewModel model = new PendingPaymentProcessViewModel();
            model.Banks = await _bankFactory.GetAll();
            model.PaymentTypes = await _paymentTypeFactory.GetAll();
            model.Poses = await _posFactory.GetAll();

            return View(model);
        }

        [Route("payments/list/success")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuccessList(string Username, string CardHolder, string CardNumber, string Token, int? BankId, int? PosId)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PagePayments))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            int _BankId;
            int _PosId;

            Username = Request.Query["Username"].ToString();
            CardHolder = Request.Query["CardHolder"].ToString();
            CardNumber = Request.Query["CardNumber"].ToString();
            Token = Request.Query["Token"].ToString();
            int.TryParse(Request.Query["BankId"].ToString(), out _BankId);
            int.TryParse(Request.Query["PosId"].ToString(), out _PosId);

            if (_BankId > 0)
                BankId = _BankId;

            if (_PosId > 0)
                PosId = _PosId;

            int draw = Convert.ToInt32(Request.Form["draw"]);// etkin sayfa numarası
            int start = Convert.ToInt32(Request.Form["start"]);//listenen ilk kayıtın  index numarası
            int length = Convert.ToInt32(Request.Form["length"]);//sayfadaki toplam listelenecek kayit sayısı
            string search = Request.Form["search[value]"];//arama

            if (!string.IsNullOrWhiteSpace(Token))
                search = Token;

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();

            int page = (start / length) + 1;

            IEnumerable<Domain.DataTransferObjects.sp_SearchPaymentProcess> payments;

            if (await _cacheFactory.PermissionControl(PanelPermissions.ProcessingForAllCompany))
            {
                payments = await _paymentProcessFactory.SearchForDataTablePaymentProcess(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, BankId, 1, PosId, IsApproved: true);
            }
            else
            {
                var loginSessionData = SessionExtensions.Get<LoginSessionData>(HttpContext.Session, "UserData");
                payments = await _paymentProcessFactory.SearchForDataTablePaymentProcess(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, BankId, 1, PosId, loginSessionData.CompanyId, IsApproved: true);
            }

            int totalRows = 0;
            totalRows = payments.Count() <= 0 ? 0 : payments.FirstOrDefault().TotalRowCount;

            return Json(new { data = payments, draw = Request.Form["draw"], recordsTotal = totalRows, recordsFiltered = totalRows });
        }

        [Route("payments/list/unsuccess")]
        public async Task<IActionResult> UnSuccessList()
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PageUnsuccessPayments))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            PendingPaymentProcessViewModel model = new PendingPaymentProcessViewModel();
            model.Banks = await _bankFactory.GetAll();
            model.PaymentTypes = await _paymentTypeFactory.GetAll();
            model.Poses = await _posFactory.GetAll();

            return View(model);
        }

        [Route("payments/list/unsuccess")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnsuccessList(string Username, string CardHolder, string CardNumber, string Token, int? BankId, int? PosId)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PagePayments))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            int _BankId;
            int _PosId;

            Username = Request.Query["Username"].ToString();
            CardHolder = Request.Query["CardHolder"].ToString();
            CardNumber = Request.Query["CardNumber"].ToString();
            Token = Request.Query["Token"].ToString();
            int.TryParse(Request.Query["BankId"].ToString(), out _BankId);
            int.TryParse(Request.Query["PosId"].ToString(), out _PosId);

            if (_BankId > 0)
                BankId = _BankId;

            if (_PosId > 0)
                PosId = _PosId;

            int draw = Convert.ToInt32(Request.Form["draw"]);// etkin sayfa numarası
            int start = Convert.ToInt32(Request.Form["start"]);//listenen ilk kayıtın  index numarası
            int length = Convert.ToInt32(Request.Form["length"]);//sayfadaki toplam listelenecek kayit sayısı
            string search = Request.Form["search[value]"];//arama

            if (!string.IsNullOrWhiteSpace(Token))
                search = Token;

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();

            int page = (start / length) + 1;

            IEnumerable<Domain.DataTransferObjects.sp_SearchPaymentProcess> payments;

            if (await _cacheFactory.PermissionControl(PanelPermissions.ProcessingForAllCompany))
            {
                payments = await _paymentProcessFactory.SearchForDataTablePaymentProcess(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, _BankId, 1, _PosId, IsApproved: false);
            }
            else
            {
                var loginSessionData = SessionExtensions.Get<LoginSessionData>(HttpContext.Session, "UserData");
                payments = await _paymentProcessFactory.SearchForDataTablePaymentProcess(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, BankId, 1, PosId, loginSessionData.CompanyId, IsApproved: false);
            }

            int totalRows = 0;
            totalRows = payments.Count() <= 0 ? 0 : payments.FirstOrDefault().TotalRowCount;

            return Json(new { data = payments, draw = Request.Form["draw"], recordsTotal = totalRows, recordsFiltered = totalRows });
        }

        [Route("payments/bank/list")]
        public async Task<IActionResult> PaymentFromBankList()
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PageWaitingForBankApprove))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            PaymentFromBankListViewModel model = new PaymentFromBankListViewModel();
            model.Banks = await _bankFactory.GetAll();
            model.PaymentTypes = await _paymentTypeFactory.GetAll();
            model.Poses = await _posFactory.GetAll();

            return View(model);
        }

        [Route("payments/bank/list")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaymentFromBankList(string Username, string CardHolder, string CardNumber, int? BankId, int? PosId)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PageWaitingForBankApprove))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            int _BankId;
            int _PosId;

            Username = Request.Query["Username"].ToString();
            CardHolder = Request.Query["CardHolder"].ToString();
            CardNumber = Request.Query["CardNumber"].ToString();
            int.TryParse(Request.Query["BankId"].ToString(), out _BankId);
            int.TryParse(Request.Query["PosId"].ToString(), out _PosId);

            if (_BankId > 0)
                BankId = _BankId;

            if (_PosId > 0)
                PosId = _PosId;

            int draw = Convert.ToInt32(Request.Form["draw"]);// etkin sayfa numarası
            int start = Convert.ToInt32(Request.Form["start"]);//listenen ilk kayıtın  index numarası
            int length = Convert.ToInt32(Request.Form["length"]);//sayfadaki toplam listelenecek kayit sayısı
            string search = Request.Form["search[value]"];//arama

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();

            int page = (start / length) + 1;

            IEnumerable<Domain.DataTransferObjects.sp_SearchPaymentProcess> payments;

            if (await _cacheFactory.PermissionControl(PanelPermissions.ProcessingForAllCompany))
            {
                payments = await _paymentProcessFactory.SearchForDataTablePaymentProcess(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, BankId, 5, PosId);
            }
            else
            {
                var loginSessionData = SessionExtensions.Get<LoginSessionData>(HttpContext.Session, "UserData");
                payments = await _paymentProcessFactory.SearchForDataTablePaymentProcess(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, BankId, 5, PosId, loginSessionData.CompanyId);
            }

            int totalRows = 0;
            totalRows = payments.Count() <= 0 ? 0 : payments.FirstOrDefault().TotalRowCount;

            return Json(new { data = payments, draw = Request.Form["draw"], recordsTotal = totalRows, recordsFiltered = totalRows });
        }

        [Route("payments/canceled-payments")]
        public async Task<IActionResult> CancelList()
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PageCancellationPayment))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            PaymentCancelListViewModel model = new PaymentCancelListViewModel();
            model.Banks = await _bankFactory.GetAll();
            model.PaymentTypes = await _paymentTypeFactory.GetAll();
            model.Poses = await _posFactory.GetAll();

            return View(model);
        }

        [Route("payments/canceled-payments")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelList(string Username, string CardHolder, string CardNumber, int? BankId, int? PosId)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PageCancellationPayment))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            int _BankId;
            int _PosId;

            Username = Request.Query["Username"].ToString();
            CardHolder = Request.Query["CardHolder"].ToString();
            CardNumber = Request.Query["CardNumber"].ToString();
            int.TryParse(Request.Query["BankId"].ToString(), out _BankId);
            int.TryParse(Request.Query["PosId"].ToString(), out _PosId);

            if (_BankId > 0)
                BankId = _BankId;

            if (_PosId > 0)
                PosId = _PosId;

            int draw = Convert.ToInt32(Request.Form["draw"]);// etkin sayfa numarası
            int start = Convert.ToInt32(Request.Form["start"]);//listenen ilk kayıtın  index numarası
            int length = Convert.ToInt32(Request.Form["length"]);//sayfadaki toplam listelenecek kayit sayısı
            string search = Request.Form["search[value]"];//arama

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();

            int page = (start / length) + 1;

            IEnumerable<Domain.DataTransferObjects.sp_SearchPaymentProcessCancel> payments;

            if (await _cacheFactory.PermissionControl(PanelPermissions.ProcessingForAllCompany))
            {
                payments = await _paymentProcessCancelFactory.SearchForDataTablePaymentProcessCancel(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, BankId, null, PosId);
            }
            else
            {
                var loginSessionData = SessionExtensions.Get<LoginSessionData>(HttpContext.Session, "UserData");
                payments = await _paymentProcessCancelFactory.SearchForDataTablePaymentProcessCancel(length, page, sortColumn, sortColumnAscDesc, search, Username, CardHolder, CardNumber, BankId, null, PosId, loginSessionData.CompanyId);
            }

            int totalRows = 0;
            totalRows = payments.Count() <= 0 ? 0 : payments.FirstOrDefault().TotalRowCount;

            return Json(new { data = payments, draw = Request.Form["draw"], recordsTotal = totalRows, recordsFiltered = totalRows });
        }

        [Route("payments/detail/{Id}")]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _Detail(int Id)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.ViewPaymentDetail))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            PaymentDetailViewModel model = new PaymentDetailViewModel();
            model.PaymentProcess = await _paymentProcessFactory.GetById(Id);
            model.PaymentType = await _paymentTypeFactory.GetById(model.PaymentProcess.PaymentTypeId.GetValueOrDefault(0));
            model.Bank = await _bankFactory.GetById(model.PaymentProcess.BankId.GetValueOrDefault(0));
            model.Pos = await _posFactory.GetById(model.PaymentProcess.PosId.GetValueOrDefault(0));
            model.Station = await _stationFactory.GetById(model.PaymentProcess.StationId.GetValueOrDefault(0));

            return PartialView(model);
        }

        [Route("payments/canceled/detail/{Id}")]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DetailCanceled(int Id)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.ViewPaymentDetail))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            PaymentCanceledDetailViewModel model = new PaymentCanceledDetailViewModel();
            model.PaymentProcessCancel = await _paymentProcessCancelFactory.GetById(Id);
            model.PaymentType = await _paymentTypeFactory.GetById(model.PaymentProcessCancel.PaymentTypeId.GetValueOrDefault(0));
            model.Bank = await _bankFactory.GetById(model.PaymentProcessCancel.BankId.GetValueOrDefault(0));
            model.Pos = await _posFactory.GetById(model.PaymentProcessCancel.PosId.GetValueOrDefault(0));
            model.Station = await _stationFactory.GetById(model.PaymentProcessCancel.StationId.GetValueOrDefault(0));

            return PartialView(model);
        }

        [Route("payments/cancel/{PaymentId}")]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _CancelPayment(int PaymentId)
        {
            CancelPaymentViewModel model = new CancelPaymentViewModel();
            var payment = await _paymentProcessFactory.GetById(PaymentId);
            if (payment != null)
            {
                switch (payment.CancellationLevel.GetValueOrDefault(0))
                {
                    case 0:
                        break;
                    case 1:
                        model.FirstCancel = true;
                        break;
                    case 2:
                        model.FirstCancel = true;
                        model.SecondCancel = true;
                        break;
                    case 3:
                        model.EndCancel = true;
                        break;
                    default:
                        break;
                }

                model.Id = payment.Id;
            }

            return PartialView(model);
        }

        [Route("payments/cancel-payment/{Id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelPayment(int Id)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PaymentRefundCancellation1)
                && !await _cacheFactory.PermissionControl(PanelPermissions.PaymentRefundCancellation2)
                && !await _cacheFactory.PermissionControl(PanelPermissions.PaymentRefundCancellation3))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            bool FirstLevelCancelPayment = HttpContext.Request.Form["FirstLevelCancelPayment"].ToString().ToLower() == "on";
            bool SecondLevelCancelPayment = HttpContext.Request.Form["SecondLevelCancelPayment"].ToString().ToLower() == "on";
            bool EndLevelCancelPayment = HttpContext.Request.Form["EndLevelCancelPayment"].ToString().ToLower() == "on";

            bool result = false;
            string message = string.Empty;

            var payment = await _paymentProcessFactory.GetById(Id);
            if (payment != null)
            {
                var loginSessionData = SessionExtensions.Get<LoginSessionData>(HttpContext.Session, "UserData");

                if (payment.CancellationLevel.GetValueOrDefault(0) == 0)
                {
                    if (await _cacheFactory.PermissionControl(PanelPermissions.PaymentRefundCancellation1) && FirstLevelCancelPayment)
                    {
                        result = await _paymentProcessFactory.UpdatePaymentCancellationLevel(Id, Core.Helpers.CancellationLevel.FirstLevel);
                        var guid = await _paymentProcessFactory.CreatePaymentToken(Id);
                        if (!string.IsNullOrWhiteSpace(guid))
                        {
                            bool email_status = false;

                            var top_users = await _userFactory.GetTopUsersByUserId(loginSessionData.UserId, 30);
                            foreach (var user in top_users)
                            {
                                var email_result = await _commonFactory.SendWaitingApprovePaymentEMail(user.EMailAddress, loginSessionData.NameSurname, guid);

                                if (!email_status)
                                    email_status = email_result.Item1;
                            }

                            if (!email_status)
                                message = "Birinci seviye ödeme iade/iptal işlemi başarılı. Ancak üst yöneticinize e-posta gönderilemedi!";
                            else
                                message = "Birinci seviye ödeme iade/iptal işlemi başarılı.";
                        }
                    }
                    else
                    {
                        message = "Birinci onay için yetkisiz erişim!";
                    }
                }
                else if (payment.CancellationLevel.GetValueOrDefault(0) == 1)
                {
                    if (await _cacheFactory.PermissionControl(PanelPermissions.PaymentRefundCancellation2) && SecondLevelCancelPayment)
                    {
                        result = await _paymentProcessFactory.UpdatePaymentCancellationLevel(Id, Core.Helpers.CancellationLevel.SecondLevel);
                        var guid = await _paymentProcessFactory.CreatePaymentToken(Id);
                        if (!string.IsNullOrWhiteSpace(guid))
                        {
                            bool email_status = false;

                            var top_users = await _userFactory.GetTopUsersByUserId(loginSessionData.UserId, 31);
                            foreach (var user in top_users)
                            {
                                var email_result = await _commonFactory.SendWaitingApprovePaymentEMail(user.EMailAddress, loginSessionData.NameSurname, guid);

                                if (!email_status)
                                    email_status = email_result.Item1;
                            }

                            if (!email_status)
                                message = "İkinci seviye ödeme iade/iptal işlemi başarılı. Ancak üst yöneticinize e-posta gönderilemedi!";
                            else
                                message = "İkinci seviye ödeme iade/iptal işlemi başarılı.";
                        }
                    }
                    else
                    {
                        message = "İkinci onay için yetkisiz erişim!";
                    }
                }
                else if (payment.CancellationLevel.GetValueOrDefault(0) == 2)
                {
                    if (await _cacheFactory.PermissionControl(PanelPermissions.PaymentRefundCancellation3) && EndLevelCancelPayment)
                    {
                        result = await _paymentProcessFactory.UpdatePaymentCancellationLevel(Id, Core.Helpers.CancellationLevel.EndLevel);
                        await _paymentProcessCancelFactory.CancelPayment(Id);
                    }
                    else
                    {
                        message = "Son onay için yetkisiz erişim!";
                    }
                }
            }
            else
            {
                message = "Ödeme kaydı bulunamadı!";
            }

            return Json(new { success = result, message = message });
        }

        //[Route("payments/cancel-payment/second/{Id}")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CancelPayment2(int Id)
        //{
        //    if (!await _cacheFactory.PermissionControl(PanelPermissions.PaymentRefundCancellation2))
        //        return RedirectToAction("UnAuthorizedAccess", "Account");

        //    var result = await _paymentProcessFactory.UpdatePaymentCancellationLevel(Id, Core.Helpers.CancellationLevel.SecondLevel);

        //    return Json(new { success = result });
        //}

        //[Route("payments/cancel-payment/end/{Id}")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CancelPayment3(int Id)
        //{
        //    if (!await _cacheFactory.PermissionControl(PanelPermissions.PaymentRefundCancellation3))
        //        return RedirectToAction("UnAuthorizedAccess", "Account");

        //    var result = await _paymentProcessCancelFactory.CancelPayment(Id);

        //    return Json(new { success = result });
        //}

    }
}
