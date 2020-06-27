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

namespace dbbuku.Controllers
{
    public class API_BukuController : ApiController
    {
        private BukuEntities db = new BukuEntities();

        // GET: api/API_Buku
        public IQueryable<tblM_Buku> GettblM_Buku()
        {
            return db.tblM_Buku;
        }

        // GET: api/API_Buku/5
        [ResponseType(typeof(tblM_Buku))]
        public IHttpActionResult GettblM_Buku(int id)
        {
            tblM_Buku tblM_Buku = db.tblM_Buku.Find(id);
            if (tblM_Buku == null)
            {
                return NotFound();
            }

            return Ok(tblM_Buku);
        }

        // PUT: api/API_Buku/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttblM_Buku(int id, tblM_Buku tblM_Buku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tblM_Buku.ID)
            {
                return BadRequest();
            }

            db.Entry(tblM_Buku).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblM_BukuExists(id))
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

        // POST: api/API_Buku
        [ResponseType(typeof(tblM_Buku))]
        public IHttpActionResult PosttblM_Buku(tblM_Buku tblM_Buku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tblM_Buku.Add(tblM_Buku);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tblM_BukuExists(tblM_Buku.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tblM_Buku.ID }, tblM_Buku);
        }

        // DELETE: api/API_Buku/5
        [ResponseType(typeof(tblM_Buku))]
        public IHttpActionResult DeletetblM_Buku(int id)
        {
            tblM_Buku tblM_Buku = db.tblM_Buku.Find(id);
            if (tblM_Buku == null)
            {
                return NotFound();
            }

            db.tblM_Buku.Remove(tblM_Buku);
            db.SaveChanges();

            return Ok(tblM_Buku);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tblM_BukuExists(int id)
        {
            return db.tblM_Buku.Count(e => e.ID == id) > 0;
        }
    }
}