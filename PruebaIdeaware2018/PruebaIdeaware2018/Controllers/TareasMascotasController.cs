using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PruebaIdeaware2018.Domain;
using PruebaIdeaware2018.Domain.Context;

namespace PruebaIdeaware2018.Controllers
{
    [Authorize]
    public class TareasMascotasController : Controller
    {
        private PruebaContext db = new PruebaContext();

        // GET: TareasMascotas
        public ActionResult Index()
        {
            var tareasMascotas = db.TareasMascotas.Include(t => t.Mascota).Include(t => t.Tarea);
            var userId = User.Identity.GetUserId().ToString();

            if (!User.IsInRole("Admin"))
            {
                tareasMascotas = tareasMascotas.Where(x => x.Mascota.ClienteId == userId);
            }

            return View(tareasMascotas.ToList());
        }

        // GET: TareasMascotas/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TareasMascota tareasMascota = db.TareasMascotas.Find(id);
            if (tareasMascota == null)
            {
                return HttpNotFound();
            }
            return View(tareasMascota);
        }

        // GET: TareasMascotas/Create
        public ActionResult Create()
        {

            IQueryable<Mascota> queryMascotas = db.Mascotas;
            var userId = User.Identity.GetUserId().ToString();
            if (!User.IsInRole("Admin"))
            {
                queryMascotas = queryMascotas.Where(x => x.ClienteId == userId);
            }

            ViewBag.MascotaId = new SelectList(queryMascotas, "Id", "Nombre");
            ViewBag.TareaId = new SelectList(db.Tareas, "Id", "Nombre");
            return View();
        }

        // POST: TareasMascotas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TareaId,MascotaId,Fecha_ejec")] TareasMascota tareasMascota)
        {
            if (ModelState.IsValid)
            {
                if (db.TareasMascotas.Count(x => x.MascotaId == tareasMascota.MascotaId && x.Fecha_ejec == tareasMascota.Fecha_ejec) < 5)
                {
                    tareasMascota.Id = Guid.NewGuid();
                    db.TareasMascotas.Add(tareasMascota);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }else
                {
                    ModelState.AddModelError("Fecha_ejec", "Solo puede crear hasta 5 tareas para esta mascota en la fecha seleccionada");
                }
            }
            IQueryable<Mascota> queryMascotas = db.Mascotas;
            var userId = User.Identity.GetUserId().ToString();
            if (!User.IsInRole("Admin"))
            {
                queryMascotas = queryMascotas.Where(x => x.ClienteId == userId);
            }

            ViewBag.MascotaId = new SelectList(queryMascotas, "Id", "Nombre", tareasMascota.MascotaId);
            ViewBag.TareaId = new SelectList(db.Tareas, "Id", "Nombre", tareasMascota.TareaId);
            return View(tareasMascota);
        }

        // GET: TareasMascotas/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TareasMascota tareasMascota = db.TareasMascotas.Find(id);
            if (tareasMascota == null)
            {
                return HttpNotFound();
            }

            IQueryable<Mascota> queryMascotas = db.Mascotas;
            var userId = User.Identity.GetUserId().ToString();
            if (!User.IsInRole("Admin"))
            {
                queryMascotas = queryMascotas.Where(x => x.ClienteId == userId);
            }

            ViewBag.MascotaId = new SelectList(queryMascotas, "Id", "Nombre", tareasMascota.MascotaId);
            ViewBag.TareaId = new SelectList(db.Tareas, "Id", "Nombre", tareasMascota.TareaId);
            return View(tareasMascota);
        }

        // POST: TareasMascotas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TareaId,MascotaId,Fecha_ejec")] TareasMascota tareasMascota)
        {
            if (ModelState.IsValid)
            {
                if (db.TareasMascotas.Count(x => x.MascotaId == tareasMascota.MascotaId && x.Fecha_ejec == tareasMascota.Fecha_ejec) < 5)
                {
                    db.Entry(tareasMascota).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Fecha_ejec", "Solo puede crear hasta 5 tareas para esta mascota en la fecha seleccionada");
                }
            }

            IQueryable<Mascota> queryMascotas = db.Mascotas;
            var userId = User.Identity.GetUserId().ToString();
            if (!User.IsInRole("Admin"))
            {
                queryMascotas = queryMascotas.Where(x => x.ClienteId == userId);
            }

            ViewBag.MascotaId = new SelectList(queryMascotas, "Id", "Nombre", tareasMascota.MascotaId);
            ViewBag.TareaId = new SelectList(db.Tareas, "Id", "Nombre", tareasMascota.TareaId);
            return View(tareasMascota);
        }

        // GET: TareasMascotas/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TareasMascota tareasMascota = db.TareasMascotas.Find(id);
            if (tareasMascota == null)
            {
                return HttpNotFound();
            }
            return View(tareasMascota);
        }

        // POST: TareasMascotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TareasMascota tareasMascota = db.TareasMascotas.Find(id);
            db.TareasMascotas.Remove(tareasMascota);
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
