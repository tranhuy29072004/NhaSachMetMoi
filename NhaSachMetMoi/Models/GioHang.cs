using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NhaSachMetMoi.Models;

namespace NhaSachMetMoi.Models
{
    public class GioHang
    {
        NhaSachEntities db = new NhaSachEntities();
        public int iMaSach { get; set; }
        public string sTenSach { get; set; }
        public string sAnhBia { get; set; }
        public double dGia { get; set; }
        public double iSale { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien
        {
            get { return iSoLuong * dGia * (1 - iSale / 100); }
        }

        public static double TongTien(List<GioHang> gioHang)
        {
            return gioHang.Sum(item => item.dThanhTien);
        }

        public GioHang(int MaSach)
        {
            iMaSach = MaSach;
            Sach sach = db.Saches.Single(n => n.MaSach == iMaSach);
            sTenSach = sach.TenSach;
            sAnhBia = sach.AnhBia;
            dGia = (double)sach.Gia;
            iSale = (double)sach.Sale;
            iSoLuong = 1;
        }
    }
}