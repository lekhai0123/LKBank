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
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace LTW4.Controllers
{
    [RequireLogin]
    public class SOTIETKIEMsController : Controller
    {
        private sotietkiemEntities db = new sotietkiemEntities();

       
        public ActionResult savetrade(string tenkh, string socmnd, string diachi, string ghichu, HttpPostedFileBase anh, string chitietstk ) 
        {
            string result = "";
            if (tenkh != "" && socmnd != "" &&  diachi != "" && ghichu != "" && chitietstk != "")
            {
                SOTIETKIEM model = new SOTIETKIEM();
                if (anh != null)
                {
                    string root = Server.MapPath("/Public/Image/");
                    string tendau = Path.GetFileNameWithoutExtension(anh.FileName);
                    string duoi = Path.GetExtension(anh.FileName);
                    string tendaydu = tendau + DateTime.Now.ToString("yyyMMddHHmmssffff") + duoi;
                    anh.SaveAs(Path.Combine(root, tendaydu));
                    model.anh = "/Public/Image/" + tendaydu;
                }
                string sosotk = "";
                var so = 1;
                for (int i = 1; i <= db.SOTIETKIEMs.Count() + 1; i++)
                {
                    if (so < 10)
                    {
                        var m = "SO" + "0" + so.ToString();
                        var n = db.SOTIETKIEMs.Find(m);
                        if (n == null)
                        {
                            sosotk = m;
                        }
                        else
                        {
                            so++;
                        }
                    }
                    else
                    {
                        var m = "SO" + so.ToString();
                        var n = db.SOTIETKIEMs.Find(m);
                        if (n == null)
                        {
                            sosotk = m;
                        }
                        else
                        {
                            so++;
                        }
                    }
                }

                model.sosotk = sosotk;
                model.ngaylap = DateTime.Now.Date;
                model.tenkh = tenkh;
                model.socmnd = socmnd;
                model.diachi = diachi;
                model.ghichu = ghichu;
                db.SOTIETKIEMs.Add(model);
                var chitietList = JsonConvert.DeserializeObject<List<CHITIET>>(chitietstk);
                foreach (var item in chitietList)
                {
                    var laisuat = db.LAISUATs.FirstOrDefault(l => l.ghichu == item.tenlaisuat);
                    if(laisuat != null)
                    {
                        var nt1 = db.NGOAITEs.SingleOrDefault(m=>m.tenngoaite == item.tenngoaite);
                        var tygia = db.TYGIAs.FirstOrDefault(t => t.mangoaite == nt1.mangoaite);
                        if(tygia != null)
                        {
                            var tongtien =item.sotiengiaodich + (item.sotiengiaodich * tygia.quyravnd * laisuat.phantramlaisuat * (laisuat.kyhan/ 12f));
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
                                    var m = "SO" + ma.ToString();
                                    var n = db.SOTIETKIEMs.Find(m);
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
                            CHITIET ct = new CHITIET();
                            ct.malaisuat = laisuat.malaisuat;
                            ct.sosotk = sosotk;
                            ct.magiaodich = magiaodich;
                            ct.ngaygiaodich = DateTime.Now.Date;
                            double kyhan = laisuat.kyhan ?? 0;
                            DateTime ngayhethan = congsothang(DateTime.Now.Date,kyhan);
                            var tenngoaite = db.NGOAITEs.SingleOrDefault(nt=>nt.tenngoaite == item.tenngoaite);
                            ct.mangoaite = tenngoaite.mangoaite;
                            ct.sotiengiaodich = item.sotiengiaodich;
                            ct.tongtiensaulai = tongtien;
                            ct.hanrut = ngayhethan;
                            db.CHITIETs.Add(ct);
                        }
                    }
                }
                db.SaveChanges();
                result = "Thành công! Giao dịch đã hoàn thành";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        static DateTime congsothang(DateTime ngaybatdau, double sothang)
        {
            if(sothang %1 ==0 )
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
        public ActionResult xoastk(string id)
        {
            try
            {
                var soso = db.SOTIETKIEMs.Find(id);
                if (soso != null)
                {
                    db.SOTIETKIEMs.Remove(soso);
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

        [HttpGet]
        public ActionResult details(string sosotk)
        {
            try
            {
                var khachhang = db.SOTIETKIEMs.Find(sosotk);
                if (khachhang == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng." }, JsonRequestBehavior.AllowGet);
                }


                var chitiets = db.CHITIETs.Where(hd => hd.sosotk == sosotk).ToList();
                var responseData = new
                {
                    anh = khachhang.anh,
                    sosotk = khachhang.sosotk,
                    ngaylap = khachhang.ngaylap.ToString("dd/MM/yyyy"),
                    tenkh = khachhang.tenkh,
                    socmnd = khachhang.socmnd,
                    diachi = khachhang.diachi,
                    ghichu = khachhang.ghichu,
                    chitiets = chitiets.Select(hd => new
                    {
                        magiaodich = hd.magiaodich,
                        ngaygiaodich =hd.ngaygiaodich.ToString("dd/MM/yyyy"),
                        malaisuat = hd.malaisuat,
                        mangoaite = hd.mangoaite,
                        tenlaisuat = hd.LAISUAT.ghichu,
                        tenngoaite = hd.NGOAITE.tenngoaite,
                        sotiengiaodich = hd.sotiengiaodich,
                        tongtiensaulai = hd.tongtiensaulai,
                        hanrut = hd.hanrut.ToString("dd/MM/yyyy"),
                    }) 
                };


                return Content(JsonConvert.SerializeObject(responseData), "application/json");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult editkh(string sosotk)
        {
            try
            {
                var kh = db.SOTIETKIEMs.Find(sosotk);
                if (kh == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng." }, JsonRequestBehavior.AllowGet);
                }
                var responseData = new
                {
                    anh = kh.anh,
                    sosotk = kh.sosotk,
                    tenkh = kh.tenkh,
                    socmnd = kh.socmnd,
                    diachi = kh.diachi,
                    ghichu = kh.ghichu,
                };
                 return Content(JsonConvert.SerializeObject(responseData), "application/json");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult capnhatkh(string sosotk, string tenkh, string socmnd, string diachi, string ghichu, HttpPostedFileBase anh2)
        {
            try
            {

                var khach = db.SOTIETKIEMs.FirstOrDefault(c => c.sosotk == sosotk);

                if (khach != null)
                {
                    khach.tenkh = tenkh;
                    khach.socmnd = socmnd;
                    khach.diachi = diachi;
                    khach.ghichu = ghichu;
                    if (anh2 != null)
                    {
                        string root = Server.MapPath("/Public/Image/");
                        string tendau = Path.GetFileNameWithoutExtension(anh2.FileName);
                        string duoi = Path.GetExtension(anh2.FileName);
                        string tendaydu = tendau + DateTime.Now.ToString("yyyMMddHHmmssffff") + duoi;
                        anh2.SaveAs(Path.Combine(root, tendaydu));
                        khach.anh = "/Public/Image/" + tendaydu;
                    }
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
            ViewBag.chitiet = db.CHITIETs.ToList();
                if (search == null) 
                {
                    List<SOTIETKIEM> danhsachsotietkiem = db.SOTIETKIEMs.ToList();
                    return View(danhsachsotietkiem);
                }           
                else
                {
                    List<SOTIETKIEM> danhsachsotietkiem = db.SOTIETKIEMs.Where(m=>m.sosotk == search || m.socmnd == search).ToList();
                    if (danhsachsotietkiem.Count() != 0) 
                    {
                        return View(danhsachsotietkiem);
                    }
                    else
                    {
                        List<SOTIETKIEM> danhsachtenkh = db.SOTIETKIEMs.Where(m=>m.tenkh.Contains(search)).ToList();
                        return View (danhsachtenkh);
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
