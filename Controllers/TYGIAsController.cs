using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using LTW4.Models;

namespace LTW4.Controllers
{
    [RequireLogin]
    public class TYGIAsController : Controller
    {
        private sotietkiemEntities db = new sotietkiemEntities();

        [HttpPost]
        public ActionResult capnhattygia(string mangoaite, double quyravnd, string ghichu)
        {
            try
            {
                var tg = db.TYGIAs.FirstOrDefault(c => c.mangoaite == mangoaite);
                if (tg != null)
                {
                    tg.ngaybatdauapdung = DateTime.Now.Date;
                    tg.quyravnd = quyravnd;
                    tg.ghichu = ghichu;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Cập nhật thành công!" });
                }

                return Json(new { success = false, message = "Không tìm thấy khách hàng!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult xoatygia(string id1)
        {
            try
            {

                var tg = db.TYGIAs.SingleOrDefault(m => m.mangoaite == id1);
                if (tg != null)
                {
                    db.TYGIAs.Remove(tg);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Giao dịch không tồn tại." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult themtygia1(string mangoaite, double? quyravnd, string ghichu)
        {
            string result = "";
            if (mangoaite != null && quyravnd != null && ghichu != null)
            {
                try
                {
                    TYGIA tg = new TYGIA
                    {
                        mangoaite = mangoaite,
                        quyravnd = quyravnd,
                        ngaybatdauapdung = DateTime.Now.Date,
                        ghichu = ghichu
                    };
                    db.TYGIAs.Add(tg);

                    db.SaveChanges();
                    result = "Thành công! Giao dịch đã hoàn thành";
                }
                catch (Exception ex)
                {
                    result = "Lỗi khi lưu dữ liệu: " + ex.Message;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: TYGIAs
        public ActionResult Index(string search)
        {

            if (search == null)
            {
                List<TYGIA> danhsachnt = db.TYGIAs.ToList();
                return View(danhsachnt);
            }
            else
            {
                List<TYGIA> danhsachnt = db.TYGIAs.Where(m => m.mangoaite.Contains(search)).ToList();
                if (danhsachnt.Count() != 0)
                {
                    return View(danhsachnt);
                }
                else
                {
                    List<TYGIA> danhsachtennt = db.TYGIAs.Where(t => t.NGOAITE.tenngoaite.Contains(search) ).ToList();
                    return View(danhsachtennt);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
