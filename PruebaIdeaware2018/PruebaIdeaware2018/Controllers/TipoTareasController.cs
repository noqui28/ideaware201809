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
    public class TipoTareasController : Controller
    {
        private PruebaContext db = new PruebaContext();

        // GET: TipoTareas
        public ActionResult Index()
        {
            return View(db.TipoTareas.ToList());
        }

        // GET: TipoTareas/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTarea tipoTarea = db.TipoTareas.Find(id);
            if (tipoTarea == null)
            {
                return HttpNotFound();
            }
            return View(tipoTarea);
        }

        // GET: TipoTareas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoTareas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,Nombre")] TipoTarea tipoTarea)
        {
            if (ModelState.IsValid)
            {
                tipoTarea.Id = Guid.NewGuid();
                db.TipoTareas.Add(tipoTarea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoTarea);
        }

        // GET: TipoTareas/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTarea tipoTarea = db.TipoTareas.Find(id);
            if (tipoTarea == null)
            {
                return HttpNotFound();
            }
            return View(tipoTarea);
        }

        // POST: TipoTareas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,Nombre")] TipoTarea tipoTarea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoTarea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoTarea);
        }

        // GET: TipoTareas/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTarea tipoTarea = db.TipoTareas.Find(id);
            if (tipoTarea == null)
            {
                return HttpNotFound();
            }
            return View(tipoTarea);
        }

        // POST: TipoTareas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TipoTarea tipoTarea = db.TipoTareas.Find(id);
            db.TipoTareas.Remove(tipoTarea);
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
