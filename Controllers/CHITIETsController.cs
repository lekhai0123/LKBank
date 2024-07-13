using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LTW4.Models;
using Newtonsoft.Json;

namespace LTW4.Controllers
{
    [RequireLogin]
    public class CHITIETsController : Controller
    {
        private sotietkiemEntities db = new sotietkiemEntities();

        // GET: CHITIETs
        [HttpPost]
        public ActionResult savetrade1(string sosotk, CHITIET[] themchitiet)
            {
            string result = "";
            if (sosotk != null && themchitiet != null && themchitiet.Length > 0)
            {
                try
                {
                    string magiaodich = "";
                    var ma = 1;
                    for (int i = 1; i <= db.CHITIETs.Count() + 1; i++)
                    {
                        if (ma < 10)
                        {
                            var m = "GD" + "0" + ma.ToString();
                            var n = db.CHITIETs.Find(m);
                            if (n == null)
                            {
                                magiaodich = m;
                            }
                            else
                            {
                                ma++;
                            }
                        }
                        else
                        {
                            var m = "GD" + ma.ToString();
                            var n = db.CHITIETs.Find(m);
                            if (n == null)
                            {
                                magiaodich = m;
                            }
                            else
                            {
                                ma++;
                            }
                        }
                    }
                    foreach (var item in themchitiet)
                    {
                        var laisuat = db.LAISUATs.FirstOrDefault(l => l.malaisuat == item.malaisuat);
                        var tygia = db.TYGIAs.FirstOrDefault(t => t.mangoaite == item.mangoaite);
                        if (laisuat != null && tygia != null)
                        {
                            var tongtien = item.sotiengiaodich + (item.sotiengiaodich * tygia.quyravnd * laisuat.phantramlaisuat * (laisuat.kyhan / 12f));
                            double kyhan = laisuat.kyhan ?? 0;
                            DateTime ngayhethan = congsothang(DateTime.Now.Date, kyhan);
                            CHITIET ct = new CHITIET
                            {
                                magiaodich = magiaodich,
                                sosotk = sosotk,
                                ngaygiaodich = DateTime.Now.Date,
                                malaisuat = item.malaisuat,
                                mangoaite = item.mangoaite,
                                sotiengiaodich = item.sotiengiaodich,
                                tongtiensaulai = tongtien,
                                hanrut = ngayhethan
                            };
                            db.CHITIETs.Add(ct);
                        }
                    }
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

        static DateTime congsothang(DateTime ngaybatdau, double sothang)
        {
            if (sothang % 1 == 0)
            {
                return ngaybatdau.AddMonths((int)sothang);
            }
            else
            {
                int songaygui = (int)(sothang * 30);
                return ngaybatdau.AddDays(songaygui);
            }
        }

        [HttpPost]
        public ActionResult xoachitiet(string id)
        {
            try
            {
                var magdich = db.CHITIETs.Find(id);
                if (magdich != null)
                {
                    db.CHITIETs.Remove(magdich);
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


        [HttpGet]
        public ActionResult editchitiet(string id)
        {
            try
            {
                var ctiet = db.CHITIETs.Find(id);
                if (ctiet == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng." }, JsonRequestBehavior.AllowGet);
                }
                var responseData = new
                {
                    tenkh = ctiet.SOTIETKIEM.tenkh,
                    sosotk = ctiet.sosotk,
                    magiaodich = ctiet.magiaodich,
                    malaisuat = ctiet.malaisuat,
                    mangoaite = ctiet.mangoaite,
                    sotiengiaodich = ctiet.sotiengiaodich,
                };
                return Content(JsonConvert.SerializeObject(responseData), "application/json");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult capnhatchitiet(string magiaodich, string sosotk, string tenkh, string malaisuat, string mangoaite, double sotiengiaodich)
        {
            try
            {
                var gd = db.CHITIETs.FirstOrDefault(c => c.magiaodich == magiaodich);
                var tg = db.TYGIAs.SingleOrDefault(m => m.mangoaite == gd.mangoaite);
                var ngaygiaodich = DateTime.Now.Date;
                var kh = db.LAISUATs.SingleOrDefault(m => m.malaisuat == malaisuat);
                double kyhan = kh.kyhan ?? 0;

                if (gd != null)
                {
                    gd.magiaodich = magiaodich;
                    gd.SOTIETKIEM.tenkh = tenkh;
                    gd.sosotk = sosotk;
                    gd.malaisuat = malaisuat;
                    DateTime hanrut = congsothang(ngaygiaodich, kyhan);
                    gd.mangoaite = mangoaite;
                    gd.sotiengiaodich = sotiengiaodich;
                    gd.ngaygiaodich = ngaygiaodich;
                    var tongtiensaulai = sotiengiaodich + (sotiengiaodich * tg.quyravnd * kh.phantramlaisuat * (kyhan/12f));
                    gd.hanrut = hanrut;
                    gd.tongtiensaulai = tongtiensaulai;
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

        public ActionResult Index(string search)
        {
            if (search == null)
            {
                List<CHITIET> danhsachgd = db.CHITIETs.ToList();
                return View(danhsachgd);
            }
            else
            {
                List<CHITIET> danhsachgd = db.CHITIETs.Where(m => m.magiaodich == search || m.sosotk == search).ToList();
                if (danhsachgd.Count() != 0)
                {
                    return View(danhsachgd);
                }
                else
                {
                    List<CHITIET> danhsachtenkh = db.CHITIETs.Where(m => m.SOTIETKIEM.tenkh.Contains(search)).ToList();
                    return View(danhsachtenkh);
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



