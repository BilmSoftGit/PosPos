using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Pospos.AdminUI.Helpers;
using Pospos.AdminUI.Models;
using Pospos.Business.Factories;
using Pospos.Core.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pospos.AdminUI.Controllers
{
    [SessionExpire]
    public class CompanyController : Controller
    {
        private readonly UserFactory _userFactory;
        private readonly CommonFactory _commonFactory;
        private readonly CacheFactory _cacheFactory;

        public CompanyController(UserFactory userFactory, CommonFactory commonFactory, CacheFactory cacheFactory)
        {
            this._userFactory = userFactory;
            this._commonFactory = commonFactory;
            this._cacheFactory = cacheFactory;
        }

        [Route("member-businesses")]
        public async Task<IActionResult> MemberBusinesses()
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PageMemberBussinesManagement))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            return View();
        }

        [Route("member-businesses")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MemberBusinessesList()
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.PageMemberBussinesManagement))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            int draw = Convert.ToInt32(Request.Form["draw"]);// etkin sayfa numarası
            int start = Convert.ToInt32(Request.Form["start"]);//listenen ilk kayıtın  index numarası
            int length = Convert.ToInt32(Request.Form["length"]);//sayfadaki toplam listelenecek kayit sayısı
            string search = Request.Form["search[value]"];//arama

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();

            int page = (start / length) + 1;

            var kullanicilar = await _userFactory.CompanySearchForDataTable(length, page, sortColumn, sortColumnAscDesc, search);
            int totalRows = 0;
            totalRows = kullanicilar.Count() <= 0 ? 0 : kullanicilar.FirstOrDefault().TotalRowCount;

            return Json(new { data = kullanicilar, draw = Request.Form["draw"], recordsTotal = totalRows, recordsFiltered = totalRows });
        }

        [Route("member-businesses/create-update")]
        public async Task<IActionResult> CreateOrUpdate(int? editId)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.CreateCompany)
                && !await _cacheFactory.PermissionControl(PanelPermissions.EditCompany))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            CreateUpdateCompanyViewModel model = new CreateUpdateCompanyViewModel();
            model.Cities = await _commonFactory.GetCities();

            if (editId.GetValueOrDefault(0) > 0)
            {
                var company = await _userFactory.GetCompanyById(editId.Value);
                if (company != null)
                {
                    model.Id = company.Id;
                    model.Address = company.Address;
                    model.CityId = company.CityId;
                    model.CompanyName = company.CompanyName;
                    model.DistrictId = company.DistrictId;
                    model.InsertDate = company.InsertDate;
                    model.IsApproved = company.IsApproved;
                    model.TaxNumber = company.TaxNumber;
                    model.TaxOffice = company.TaxOffice;
                    model.CompanyCode = company.CompanyCode;
                }
            }

            return View(model);
        }

        [Route("member-businesses/create-update")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrUpdate(int? editId, string CompanyName, string TaxOffice, string TaxNumber, int CityId, int DistrictId, string Address, string CompanyCode)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.CreateCompany)
                && !await _cacheFactory.PermissionControl(PanelPermissions.EditCompany))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            CreateUpdateCompanyViewModel model = new CreateUpdateCompanyViewModel();
            model.Cities = await _commonFactory.GetCities();

            bool IsApproved = HttpContext.Request.Form["IsApproved"].ToString().ToLower() == "on";

            if (!editId.HasValue || editId.Value <= 0)
            {
                var company = await _userFactory.GetCompaniesByTaxNumber(TaxNumber);
                if (company == null)
                {
                    company = await _userFactory.InsertCompany(CompanyName, TaxOffice, TaxNumber, Address, CityId, DistrictId, IsApproved, CompanyCode);
                    if (company != null)
                    {
                        model.Success = true;
                        model.Message = "Kayıt başarıyla alındı.";
                    }
                    else
                    {
                        model.Message = "Firma kaydı alınırken hata oluştu.";
                    }
                }
                else
                {
                    model.Message = "Firma zaten kayıtlı.";
                }
            }
            else
            {
                var company = await _userFactory.GetCompanyById(editId.Value);
                if (company != null)
                {
                    company.Address = Address;
                    company.CityId = CityId;
                    company.CompanyName = CompanyName;
                    company.DistrictId = DistrictId;
                    company.IsApproved = IsApproved;
                    company.TaxNumber = TaxNumber;
                    company.TaxOffice = TaxOffice;

                    if (await _userFactory.UpdateCompany(company))
                    {
                        model.Success = true;
                        model.Message = "Kayıt başarıyla güncellendi.";
                    }
                }
                else
                {
                    model.Message = "Güncellenecek kayıt bulunamadı.";
                }
            }

            return View(model);
        }

        [Route("company/active-passive-company")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivePassiveCompany(int Id, bool IsApproved)
        {
            if (!await _cacheFactory.PermissionControl(PanelPermissions.ApproveCompany))
                return RedirectToAction("UnAuthorizedAccess", "Account");

            if (await _userFactory.MakeCompanyApprove(Id, IsApproved))
                return Json(new { success = true });


            return Json(new { success = false });
        }

    }
}
