using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PruebaIdeaware2018.Domain;
using PruebaIdeaware2018.Domain.Context;

namespace PruebaIdeaware2018.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TiendasController : Controller
    {
        private PruebaContext db = new PruebaContext();

        // GET: Tiendas
        public ActionResult Index()
        {
            var tiendas = db.Tiendas.Include(t => t.Ciudad);
            return View(tiendas.ToList());
        }

        // GET: Tiendas/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tienda tienda = db.Tiendas.Find(id);
            if (tienda == null)
            {
                return HttpNotFound();
            }
            return View(tienda);
        }

        // GET: Tiendas/Create
        public ActionResult Create()
        {
            ViewBag.CiudadId = new SelectList(db.Ciudades, "Id", "Nombre");
            return View();
        }

        // POST: Tiendas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,Nombre,CiudadId")] Tienda tienda)
        {
            if (ModelState.IsValid)
            {
                tienda.Id = Guid.NewGuid();
                db.Tiendas.Add(tienda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CiudadId = new SelectList(db.Ciudades, "Id", "Nombre", tienda.CiudadId);
            return View(tienda);
        }

        // GET: Tiendas/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tienda tienda = db.Tiendas.Find(id);
            if (tienda == null)
            {
                return HttpNotFound();
            }
            ViewBag.CiudadId = new SelectList(db.Ciudades, "Id", "Nombre", tienda.CiudadId);
            return View(tienda);
        }

        // POST: Tiendas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,Nombre,CiudadId")] Tienda tienda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tienda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CiudadId = new SelectList(db.Ciudades, "Id", "Nombre", tienda.CiudadId);
            return View(tienda);
        }

        // GET: Tiendas/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tienda tienda = db.Tiendas.Find(id);
            if (tienda == null)
            {
                return HttpNotFound();
            }
            return View(tienda);
        }

        // POST: Tiendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Tienda tienda = db.Tiendas.Find(id);
            db.Tiendas.Remove(tienda);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
