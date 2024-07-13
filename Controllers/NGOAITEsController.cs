using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LTW4.Models;
using Newtonsoft.Json;

namespace LTW4.Controllers
{
    [RequireLogin]
    public class NGOAITEsController : Controller
    {
        private sotietkiemEntities db = new sotietkiemEntities();

        public ActionResult savengoaite(string mangoaite, string tenngoaite, double? quyravnd, string ghichu)
        {
            string result = "";
            if (mangoaite != "" && tenngoaite != "" && quyravnd != null && ghichu != "" )
            {
                NGOAITE model = new NGOAITE();             
                model.mangoaite = mangoaite;
                model.tenngoaite = tenngoaite;                
                db.NGOAITEs.Add(model);
                TYGIA tg = new TYGIA();
                tg.mangoaite = mangoaite;
                tg.ngaybatdauapdung = DateTime.Now.Date;
                tg.quyravnd = quyravnd;
                tg.ghichu = ghichu;
                db.TYGIAs.Add(tg);
                db.SaveChanges();
                result = "Thành công! Giao dịch đã hoàn thành";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult capnhatngoaite(string mangoaite, string tenngoaite)
        {
            try
            {
                var nt = db.NGOAITEs.FirstOrDefault(c => c.mangoaite == mangoaite);

                if (nt != null)
                {
                    nt.mangoaite = mangoaite;
                    nt.tenngoaite=tenngoaite;   
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

        [HttpGet]
        public ActionResult details(string mangoaite)
        {
            try
            {
                var nt = db.NGOAITEs.Find(mangoaite);
                if (nt == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng." }, JsonRequestBehavior.AllowGet);
                }
                var tygia = db.TYGIAs.Where(hd => hd.mangoaite == mangoaite).ToList();
                var responseData = new
                {
                    mangoaite = mangoaite,
                    tenngoaite = nt.tenngoaite,
                    tygia = tygia.Select(hd => new
                    {
                        mangoaite= mangoaite,
                        ngaybatdauapdung = hd.ngaybatdauapdung.ToString("dd/MM/yyyy"),
                        quyravnd = hd.quyravnd,
                        ghichu = hd.ghichu,
                    })
                };
                return Content(JsonConvert.SerializeObject(responseData), "application/json");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult xoangoaite(string id)
        {
            try
            {
                var nt = db.NGOAITEs.Find(id);
                if (nt != null)
                {
                    db.NGOAITEs.Remove(nt);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Khách hàng không tồn tại." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        // GET: NGOAITEs
        public ActionResult Index(string search)
        {
            if (search == null)
            {
                List<NGOAITE> danhsachnt = db.NGOAITEs.ToList();
                return View(danhsachnt);
            }
            else
            {
                List<NGOAITE> danhsachnt = db.NGOAITEs.Where(m => m.mangoaite.Contains(search)).ToList();
                if (danhsachnt.Count() != 0)
                {
                    return View(danhsachnt);
                }
                else
                {
                    List<NGOAITE> danhsachtennt = db.NGOAITEs.Where(m => m.tenngoaite.Contains(search)).ToList();
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
