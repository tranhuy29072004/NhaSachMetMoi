using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhaSachMetMoi.Models;

namespace NhaSachMetMoi.Controllers
{
    public class DangNhapController : Controller
    {
        NhaSachEntities db = new NhaSachEntities();

        // GET: DangNhap
        public ActionResult IndexTK()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection F)
        {
            string sTaiKhoan = F["txtTaiKhoan"].ToString();
            string sMatKhau = F["txtMatKhau"].ToString();

            Admin admin = db.Admins.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);

            if (admin != null)
            {
                Session["TaiKhoan"] = admin;
                return RedirectToAction("IndexAd", "Admin");
            }

            KH kh = db.KHs.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);

            if (kh != null)
            {
                ViewBag.ThongBao = "Đăng nhập thành công";
                Session["TaiKhoan"] = kh;
                return RedirectToAction("Index", "TrangChu");
            }

            ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu không đúng";
            return View("IndexTK");
        }

        [HttpPost]
        public ActionResult DangKy(KH kh)
        {
            if (ModelState.IsValid)
            {
                db.KHs.Add(kh);
                db.SaveChanges();
                ViewBag.ThongBao = "Đăng ký thành công";
                return View("IndexTK");
            }

            return View("IndexTK");
        }
        public ActionResult Logout()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("IndexTK");
        }
    }
}