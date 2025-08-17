using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using NhaSachMetMoi.Models;

namespace NhaSachMetMoi.Controllers
{
    public class KhachHangController : Controller
    {
        NhaSachEntities db = new NhaSachEntities();

        public ActionResult ThongTinCaNhan(int id)
        {
            var khachHang = db.KHs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        public ActionResult SuaTTCN(int id)
        {
            var khachHang = db.KHs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaTTCN(KH khachHang)
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
                    khachHangInDb.NgaySinh = khachHang.NgaySinh;

                    db.SaveChanges();
                }
                return RedirectToAction("ThongTinCaNhan", new { id = khachHang.MaKH });
            }
            return View(khachHang);
        }

        public ActionResult XoaTTCN(int id)
        {
            var khachHang = db.KHs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            db.KHs.Remove(khachHang);
            db.SaveChanges();
            return RedirectToAction("Index", "TrangChu");
        }

        public ActionResult LichSuDonHang(int id)
        {
            var donHangs = db.DonHangs.Where(dh => dh.MaKH == id).ToList();
            if (donHangs == null)
            {
                return HttpNotFound();
            }
            return View(donHangs);
        }

        public ActionResult ChiTietDonHang(int id)
        {
            var donHang = db.DonHangs.Include("ChiTietDHs.Sach").FirstOrDefault(dh => dh.MaDH == id);
            if (donHang == null)
            {
                return HttpNotFound();
            }

            // Lấy mã giảm giá từ đơn hàng
            var maGiam = db.MaGiamGias.FirstOrDefault(mg => mg.id == donHang.MaGiamGia);
            int giamGia = maGiam != null ? (int)maGiam.giam : 0;

            ViewBag.giamGia = giamGia;
            return View(donHang);
        }






    }
}
