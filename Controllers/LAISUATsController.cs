using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LTW4.Models;

namespace LTW4.Controllers
{
    [RequireLogin]
    public class LAISUATsController : Controller
    {
        private sotietkiemEntities db = new sotietkiemEntities();

        [HttpPost]
        public ActionResult savelaisuat(string malaisuat,double? kyhan, double? phantramlaisuat, string ghichu )
        {
            string result = "";
            if (malaisuat != null && kyhan != null && phantramlaisuat != null && ghichu != null)
            {
                try
                {
                    LAISUAT ls = new LAISUAT
                    {
                        malaisuat = malaisuat,
                        kyhan = kyhan,
                        phantramlaisuat = phantramlaisuat,  
                        ghichu = ghichu
                    };
                    db.LAISUATs.Add(ls);
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

        [HttpPost]
        public ActionResult capnhatlaisuat(string malaisuat, double kyhan, double phantramlaisuat, string ghichu)
        {
            try
            {
                var ls = db.LAISUATs.FirstOrDefault(c => c.malaisuat == malaisuat);

                if (ls != null)
                {
                    ls.kyhan = kyhan;
                    ls.phantramlaisuat = phantramlaisuat;
                    ls.ghichu = ghichu;
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
        public ActionResult xoalaisuat(string id)
        {
            try
            {
                var mals = db.LAISUATs.Find(id);
                if (mals != null)
                {
                    db.LAISUATs.Remove(mals);
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

        // GET: LAISUATs
        public ActionResult Index(string search)
        {
            ViewBag.chitiet = db.CHITIETs.ToList();
            if (search == null)
            {
                List<LAISUAT> danhsachlaisuat = db.LAISUATs.ToList();
                return View(danhsachlaisuat);
            }
            else
            {
                List<LAISUAT> danhsachlaisuat = db.LAISUATs.Where(m => m.malaisuat == search).ToList();

                if (danhsachlaisuat.Count() != 0)
                {
                    return View(danhsachlaisuat);
                }
                else
                {                    
                    List<LAISUAT> danhsachghichu = db.LAISUATs.Where(m => m.ghichu.Contains(search)).ToList();
                    return View(danhsachghichu);
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
