using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using dbbuku.Models;
using Microsoft.AspNet.Identity;

namespace dbbuku.Controllers
{
    public class API_TransaksiController : ApiController
    {
        private BukuEntities db = new BukuEntities();

        // GET: api/API_Transaksi
        public IQueryable<TransaksiModel> GetTblM_Transaksi()
        {
            string UserId = User.Identity.GetUserId();
            List<TblM_Transaksi> ls = db.TblM_Transaksi.Where(m => m.UserId == UserId).ToList();

            List<TransaksiModel> transaksiLs = new List<TransaksiModel>();
            foreach (TblM_Transaksi item in ls)
            {
                TransaksiModel model = new TransaksiModel();

                string NamaBuku = db.tblM_Buku.Where(x => x.ID == item.BukuId).FirstOrDefault().Nama;

                model.ID = item.ID;
                model.NamaBuku = NamaBuku;
                model.Jumlah = item.Jumlah;
                model.Total = item.Total;

                transaksiLs.Add(model);
            }

            return transaksiLs.AsQueryable();
        }

        // GET: api/API_Transaksi/5
        [ResponseType(typeof(TblM_Transaksi))]
        public IHttpActionResult GetTblM_Transaksi(int id)
        {
            TblM_Transaksi tblM_Transaksi = db.TblM_Transaksi.Find(id);
            if (tblM_Transaksi == null)
            {
                return NotFound();
            }

            return Ok(tblM_Transaksi);
        }

        // PUT: api/API_Transaksi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTblM_Transaksi(int id, TblM_Transaksi tblM_Transaksi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tblM_Transaksi.ID)
            {
                return BadRequest();
            }

            db.Entry(tblM_Transaksi).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblM_TransaksiExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/API_Transaksi
        [ResponseType(typeof(TblM_Transaksi))]
        public IHttpActionResult PostTblM_Transaksi(TblM_Transaksi tblM_Transaksi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string UserId = User.Identity.GetUserId();
            tblM_Transaksi.UserId = UserId;

            //Hitung
            decimal? harga = db.tblM_Buku.Where(x => x.ID == tblM_Transaksi.BukuId).FirstOrDefault().Harga;
            tblM_Transaksi.Total = tblM_Transaksi.Jumlah * harga;

            WebService ws = new WebService("http://www.dneonline.com/calculator.asmx", "getBillingCustomer");
            //ws.Params.Add("vkdunit", coll.UnitID);
            //ws.Invoke();
            //var xnList = ws.ResultXML.Descendants("Table");
            //DataTable dt = Util.XmlConvertToDataTable(xnList);
            //

            db.TblM_Transaksi.Add(tblM_Transaksi);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tblM_Transaksi.ID }, tblM_Transaksi);
        }

        // DELETE: api/API_Transaksi/5
        [ResponseType(typeof(TblM_Transaksi))]
        public IHttpActionResult DeleteTblM_Transaksi(int id)
        {
            TblM_Transaksi tblM_Transaksi = db.TblM_Transaksi.Find(id);
            if (tblM_Transaksi == null)
            {
                return NotFound();
            }

            db.TblM_Transaksi.Remove(tblM_Transaksi);
            db.SaveChanges();

            return Ok(tblM_Transaksi);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TblM_TransaksiExists(int id)
        {
            return db.TblM_Transaksi.Count(e => e.ID == id) > 0;
        }
    }

    public class TransaksiModel
    {
        public int ID { get; set; }
        public string NamaBuku { get; set; }
        public int? Jumlah { get; set; }
        public decimal? Total { get; set; }
    }
}