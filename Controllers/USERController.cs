using LTW4.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace LTW4.Controllers
{
   
    public class USERController : Controller
    {
        // GET: USER
        sotietkiemEntities db = new sotietkiemEntities();
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = db.USERs.Where(s =>(s.username.Equals(email) || s.email.Equals(email)) && s.password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    Session["Username"] = data.FirstOrDefault().username;
                    Session["Email"] = data.FirstOrDefault().email;
                    Session["Password"] = data.FirstOrDefault().password;
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("username", "Sai tài khoản hoặc mật khẩu");
                    return View();
                }
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult signUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult signUp(USER user)
        {
            if (ModelState.IsValid)
            {
                var check = db.USERs.FirstOrDefault(s => s.email == user.email);
                if (check == null)
                {
                    if (IsStrongPassword(user.password))
                    {
                        user.password = GetMD5(user.password);
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.USERs.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("Login");
                    }
                    else 
                    {
                        ModelState.AddModelError("password", "Mật khẩu phải chứa ít nhất 8 ký tự, trong đó bao gồm ký tự in hoa");
                        return View();
                    }
                }
                else
                {
                    ViewBag.error = "Email ";
                    return View();
                }
            }
            return View();
        }
        private bool IsStrongPassword(string password)
        {
            if (password.Length < 8)
                return false;
            if (!Regex.IsMatch(password, "[A-Z]"))
                return false;
            return true;
        }
        [AllowAnonymous]
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}