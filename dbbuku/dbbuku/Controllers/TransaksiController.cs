using dbbuku.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbbuku.Controllers
{
    public class TransaksiController : Controller
    {
        // GET: Transaksi
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBuku()
        {
            BukuEntities db = new BukuEntities();
            var coll = db.tblM_Buku.ToList();
            return Json(coll, JsonRequestBehavior.AllowGet);
        }
    }
}