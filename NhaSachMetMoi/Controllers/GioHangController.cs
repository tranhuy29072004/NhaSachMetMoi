using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhaSachMetMoi.Models;

namespace NhaSachMetMoi.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        NhaSachEntities db = new NhaSachEntities();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int iMaSach, string strURL)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == iMaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.Find(n => n.iMaSach == iMaSach);
            if (sanpham == null)
            {
                sanpham = new GioHang(iMaSach);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }
        public ActionResult MuaNgay(int iMaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == iMaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.Find(n => n.iMaSach == iMaSach);

            if (sanpham == null)
            {
                sanpham = new GioHang(iMaSach);
                lstGioHang.Add(sanpham);
            }
            else
            {
                sanpham.iSoLuong++;
            }
            return RedirectToAction("GioHang", "GioHang");
        }
        public ActionResult CapNhatGioHang(int iMaSach, int txtSoLuong)
        {
            List<GioHang> gioHang = Session["GioHang"] as List<GioHang>;
            GioHang item = gioHang.FirstOrDefault(x => x.iMaSach == iMaSach);
            if (item != null)
            {
                if (txtSoLuong < 1)
                {
                    txtSoLuong = 1;
                }
                item.iSoLuong = txtSoLuong;
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang(int iMaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == iMaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.iMaSach == iMaSach);
            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "TrangChu");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "TrangChu");
            }
            List<GioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return dTongTien;
        }
        public ActionResult GioHangPartial()
        {
            if (TongSoLuong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult SuaGioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "TrangChu");
            }
            List<GioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        public ActionResult XacNhanDonHang()
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("IndexTK", "DangNhap");
            }
            if (Session["GioHang"] == null)
            {
                RedirectToAction("Index", "Trangchu");
            }
            List<GioHang> lstGioHang = LayGioHang();

            ViewBag.lstGioHang = lstGioHang;

            return View(Session["TaiKhoan"]);
        }
        [HttpPost]
        public ActionResult XacNhanDonHang(FormCollection F)
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("IndexTK", "DangNhap");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Trangchu");
            }

            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.lstGioHang = lstGioHang;

            if (F != null && !string.IsNullOrEmpty(F["maCode"]))
            {
                string maCode = F["maCode"].ToString();

                if (maCode.Length <= 0)
                {
                    ViewBag.ThongBao = "Nhập mã";
                    return View(Session["TaiKhoan"]);
                }

                try
                {
                    MaGiamGia maGiam = db.MaGiamGias.FirstOrDefault(item => item.ma == maCode);
                    if (maGiam != null)
                    {
                        ViewBag.giamGia = maGiam.giam;
                        Session["maGiam"] = maGiam.giam;
                    }
                    else
                    {
                        ViewBag.giamGia = null;
                        ViewBag.ThongBao = "Mã không tồn tại";
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log chi tiết lỗi hoặc hiển thị thông báo lỗi tùy chỉnh
                    ViewBag.ThongBao = "Đã xảy ra lỗi: " + ex.Message;
                    if (ex.InnerException != null)
                    {
                        ViewBag.ThongBao += " - Inner Exception: " + ex.InnerException.Message;
                    }
                    return View(Session["TaiKhoan"]);
                }
            }

            return View(Session["TaiKhoan"]);
        }

        public ActionResult SuaXacNhanTT(int id)
        {
            var khachHang = db.KHs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        [HttpPost]
        public ActionResult SuaXacNhanTT(KH khachHang)
        {

            if (ModelState.IsValid)
            {
                var khachHangInDb = db.KHs.Find(khachHang.MaKH);
                if (khachHangInDb != null)
                {
                    // Chỉ cập nhật các trường cần thiết, không cập nhật TaiKhoan và MatKhau
                    khachHangInDb.HoTen = khachHang.HoTen;
                    khachHangInDb.Email = khachHang.Email;
                    khachHangInDb.DiaChi = khachHang.DiaChi;
                    khachHangInDb.DienThoai = khachHang.DienThoai;


                    db.SaveChanges();
                }
                return RedirectToAction("XacNhanDonHang", new { id = khachHang.MaKH });

            }
            return View(khachHang);
        }

        public ActionResult Dathangthanhcong()
        {
            DonHang ddh = new DonHang();
            KH kh = (KH)Session["taikhoan"];
            List<GioHang> gh = LayGioHang();
            int phanTramGiam = 0;

            if (Session["maGiam"] != null)
            {
                phanTramGiam = Convert.ToInt32(Session["maGiam"]);
                ddh.MaGiamGia = Convert.ToInt32(Session["maGiamId"]); // Lưu Id của mã giảm giá
            }

            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            double tongTien = TongTien();
            double tienTru = tongTien * ((double)phanTramGiam / 100);
            double tienGiam = tongTien - tienTru;
            ddh.TongTien = (int)tienGiam;
            db.DonHangs.Add(ddh);
            db.SaveChanges();
            foreach (var item in gh)
            {
                ChiTietDH ctdh = new ChiTietDH();
                ctdh.MaDH = ddh.MaDH;
                ctdh.MaSach = item.iMaSach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.Gia = (decimal)item.dGia;
                db.ChiTietDHs.Add(ctdh);
            }
            db.SaveChanges();
            Session["giohang"] = null;
            return View("Dathangthanhcong");
        }


    }
}
