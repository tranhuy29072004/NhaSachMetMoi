using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhaSachMetMoi.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace NhaSachMetMoi.Controllers
{
    public class AdminController : Controller
    {
        NhaSachEntities db = new NhaSachEntities();

        // GET: Admin
        public ActionResult IndexAd(int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(db.Saches.ToList().OrderBy(n => n.MaSach).ToPagedList(pageNum, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaTL = new SelectList(db.TLs.ToList(), "MaTL", "TenTL");
            ViewBag.MaTG = new SelectList(db.TGs.ToList(), "MaTG", "TenTG");
            ViewBag.MaNXB = new SelectList(db.NXBs.ToList(), "MaNXB", "TenNXB");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sach sach, HttpPostedFileBase fileupload)
        {
            if (sach.Gia < 0)
            {
                ModelState.AddModelError("Gia", "Giá không thể là số âm.");
            }

            if (sach.Sale < 0)
            {
                ModelState.AddModelError("Sale", "Khuyến mãi không thể là số âm.");
            }

            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(fileupload.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/images/sach"), fileName);
                fileupload.SaveAs(path);
                sach.AnhBia = fileupload.FileName;
                db.Saches.Add(sach);
                db.SaveChanges();
                return RedirectToAction("IndexAd");
            }

            ViewBag.MaTL = new SelectList(db.TLs.ToList(), "MaTL", "TenTL");
            ViewBag.MaTG = new SelectList(db.TGs.ToList(), "MaTG", "TenTG");
            ViewBag.MaNXB = new SelectList(db.NXBs.ToList(), "MaNXB", "TenNXB");
            return View(sach);
        }

        [HttpGet]
        public ActionResult Edit(int MaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaTL = new SelectList(db.TLs.ToList(), "MaTL", "TenTL");
            ViewBag.MaTG = new SelectList(db.TGs.ToList(), "MaTG", "TenTG");
            ViewBag.MaNXB = new SelectList(db.NXBs.ToList(), "MaNXB", "TenNXB");
            return View(sach);
        }

        [HttpPost]
        public ActionResult Edit(Sach sach, HttpPostedFileBase fileupload)
        {
            if (sach.Gia < 0)
            {
                ModelState.AddModelError("Gia", "Giá không thể là số âm.");
            }

            if (sach.Sale < 0)
            {
                ModelState.AddModelError("Sale", "Khuyến mãi không thể là số âm.");
            }

            if (ModelState.IsValid)
            {
                if (fileupload != null && fileupload.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images/sach"), fileName);
                    fileupload.SaveAs(path);
                    sach.AnhBia = fileName;
                }
                else
                {
                    // Giữ nguyên ảnh bìa hiện tại nếu không có tệp mới được chọn
                    var currentSach = db.Saches.AsNoTracking().FirstOrDefault(s => s.MaSach == sach.MaSach);
                    if (currentSach != null)
                    {
                        sach.AnhBia = currentSach.AnhBia;
                    }
                }

                db.Entry(sach).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexAd");
            }

            ViewBag.MaTL = new SelectList(db.TLs.ToList(), "MaTL", "TenTL");
            ViewBag.MaTG = new SelectList(db.TGs.ToList(), "MaTG", "TenTG");
            ViewBag.MaNXB = new SelectList(db.NXBs.ToList(), "MaNXB", "TenNXB");
            return View(sach);
        }

        public ActionResult Details(int MaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        public ActionResult Delete(int MaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult XacNhan(int MaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Xóa tất cả các bản ghi liên quan trong bảng ChiTietDH
            var chiTietDonHang = db.ChiTietDHs.Where(c => c.MaSach == MaSach).ToList();
            foreach (var chiTiet in chiTietDonHang)
            {
                db.ChiTietDHs.Remove(chiTiet);
            }

            db.Saches.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("IndexAd");
        }

        public ActionResult IndexDH(int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(db.DonHangs.ToList().OrderBy(n => n.MaDH).ToPagedList(pageNum, pageSize));
        }
        [HttpGet]
        public ActionResult EditDH(int MaDH)
        {
            DonHang donHang = db.DonHangs.SingleOrDefault(n => n.MaDH == MaDH);

            if (donHang == null)
            {
                Response.StatusCode = 404;
            }

            return View(donHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDH(DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexDH");
            }
            return View(donHang);
        }

        public ActionResult DetailDH(int MaDH)
        {
            DonHang donHang = db.DonHangs.SingleOrDefault(n => n.MaDH == MaDH);

            if (donHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(donHang);
        }

        [HttpPost]
        public ActionResult DetailDH(int MaDH, bool xacnhan, DateTime? ngaygiao)
        {
            DonHang donHang = db.DonHangs.SingleOrDefault(n => n.MaDH == MaDH);

            if (donHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            if (xacnhan && ngaygiao.HasValue)
            {
                donHang.NgayGiao = ngaygiao.Value; // Cập nhật ngày giao
                db.Entry(donHang).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return View(donHang);
        }

        public ActionResult DeleteDH(int MaDH)
        {
            DonHang donHang = db.DonHangs.SingleOrDefault(n => n.MaDH == MaDH);
            if (donHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(donHang);
        }
        [HttpPost, ActionName("DeleteDH")]
        public ActionResult XacNhanDH(int MaDH)
        {
            DonHang donHang = db.DonHangs.SingleOrDefault(n => n.MaDH == MaDH);

            if (donHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var chiTietDHs = db.ChiTietDHs.Where(ct => ct.MaDH == MaDH);
            foreach (var chiTietDH in chiTietDHs)
            {
                db.ChiTietDHs.Remove(chiTietDH);
            }
            db.DonHangs.Remove(donHang);
            db.SaveChanges();

            return RedirectToAction("IndexDH");
        }

        public ActionResult IndexKH(int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(db.KHs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNum, pageSize));
        }

        public ActionResult EditKH(int MaKH)
        {
            KH kh = db.KHs.SingleOrDefault(n => n.MaKH == MaKH);

            if (kh == null)
            {
                Response.StatusCode = 404;
            }

            return View(kh);
        }

        [HttpPost]
        public ActionResult EditKH(KH kh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kh).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexKH");
            }
            return View(kh);
        }

        public ActionResult DetailKH(int MaKH)
        {
            KH kh = db.KHs.SingleOrDefault(n => n.MaKH == MaKH);

            if (kh == null)
            {
                Response.StatusCode = 404;
            }
            return View(kh);
        }

        public ActionResult DeleteKH(int MaKH)
        {
            KH kh = db.KHs.SingleOrDefault(n => n.MaKH == MaKH);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpPost, ActionName("DeleteKH")]
        public ActionResult XacNhanKH(int MaKH)
        {
            KH kh = db.KHs.SingleOrDefault(n => n.MaKH == MaKH);

            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var donHangs = db.DonHangs.Where(dh => dh.MaKH == MaKH);
            foreach (var donHang in donHangs)
            {
                var chiTietDHs = db.ChiTietDHs.Where(ct => ct.MaDH == donHang.MaDH);
                foreach (var chiTietDH in chiTietDHs)
                {
                    db.ChiTietDHs.Remove(chiTietDH);
                }
                db.DonHangs.Remove(donHang);
            }
            db.KHs.Remove(kh);
            db.SaveChanges();
            return RedirectToAction("IndexKH");
        }

        [HttpPost]
        public ActionResult AddMaTL(string TenTL)
        {
            var loaiSach = new TL { TenTL = TenTL };
            db.TLs.Add(loaiSach);
            db.SaveChanges();
            return RedirectToAction("Create");
        }

        [HttpPost]
        public ActionResult AddMaTG(string TenTG)
        {
            var tacGia = new TG { TenTG = TenTG };
            db.TGs.Add(tacGia);
            db.SaveChanges();
            return RedirectToAction("Create");
        }

        [HttpPost]
        public ActionResult AddMaNXB(string TenNXB)
        {
            var nhaXuatBan = new NXB { TenNXB = TenNXB };
            db.NXBs.Add(nhaXuatBan);
            db.SaveChanges();
            return RedirectToAction("Create");
        }
    }
}
