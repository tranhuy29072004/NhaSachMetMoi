using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhaSachMetMoi.Models;
using PagedList.Mvc;
using PagedList;

namespace NhaSachMetMoi.Controllers
{
    public class TimController : Controller
    {
        // GET: Tim
        NhaSachEntities db = new NhaSachEntities();

        [HttpPost]
        public ActionResult Ketquatimkiem(FormCollection f, int? page)
        {
            string sTukhoa = f["txtTim"] != null ? f["txtTim"].ToString() : string.Empty;
            string tenTL = f["TenTL"] != null ? f["TenTL"].ToString() : string.Empty;
            ViewBag.Tukhoa = sTukhoa;
            ViewBag.TenTL = tenTL;

            var lsttim = db.Saches.AsQueryable();

            if (!string.IsNullOrEmpty(sTukhoa))
            {
                lsttim = lsttim.Where(n => n.TenSach.Contains(sTukhoa));
            }

            if (!string.IsNullOrEmpty(tenTL))
            {
                lsttim = lsttim.Where(n => n.TL.TenTL.Contains(tenTL));
            }

            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(lsttim.OrderBy(n => n.TenSach).ToPagedList(pageNum, pageSize));
        }

        [HttpGet]
        public ActionResult Ketquatimkiem(int? page, string sTukhoa, string tenTL)
        {
            ViewBag.Tukhoa = sTukhoa;
            ViewBag.TenTL = tenTL;

            var lsttim = db.Saches.AsQueryable();

            if (!string.IsNullOrEmpty(sTukhoa))
            {
                lsttim = lsttim.Where(n => n.TenSach.Contains(sTukhoa));
            }

            if (!string.IsNullOrEmpty(tenTL))
            {
                lsttim = lsttim.Where(n => n.TL.TenTL.Contains(tenTL));
            }

            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(lsttim.OrderBy(n => n.TenSach).ToPagedList(pageNum, pageSize));
        }
    }
}
