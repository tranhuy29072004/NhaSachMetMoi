using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using NhaSachMetMoi.Models;
// Main
namespace NhaSachMetMoi.Controllers
{
    public class TrangchuController : Controller
    {
        // GET: Trangchu
        NhaSachEntities db = new NhaSachEntities();
        public ActionResult Index()
        {
            var lstSach = db.Saches.ToList();
            ViewBag.DanhMucs = db.TLs.ToList();
            return View(lstSach);
            
             
        }
        public ActionResult SachTL(int MaTL, int? page, string sortOrder)
        {
            ViewBag.DanhMucs = db.TLs.ToList();
            int pageSize = 12;
            int pageNum = (page ?? 1);
            TL tl = db.TLs.SingleOrDefault(n => n.MaTL == MaTL);
            List<Sach> lstSach = db.Saches.Where(n => n.MaTL == MaTL).ToList();
            switch (sortOrder)
            {
                case "PriceAsc":
                    lstSach = lstSach.OrderBy(s => s.Gia).ToList();
                    break;
                case "PriceDesc":
                    lstSach = lstSach.OrderByDescending(s => s.Gia).ToList();
                    break;
                case "NameAsc":
                    lstSach = lstSach.OrderBy(s => s.TenSach).ToList();
                    break;
                case "NameDesc":
                    lstSach = lstSach.OrderByDescending(s => s.TenSach).ToList();
                    break;
                default:
                    break;
            }
            ViewBag.Page = pageNum;
            ViewBag.CurrentSort = sortOrder;

            return View(lstSach.ToPagedList(pageNum, pageSize));

        }
        public ActionResult XemCT(int MaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            return View(sach);

        }
         
    }
}