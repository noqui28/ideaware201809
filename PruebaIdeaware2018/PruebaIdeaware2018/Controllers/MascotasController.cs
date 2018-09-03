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
using PruebaIdeaware2018.Models;

namespace PruebaIdeaware2018.Controllers
{
    [Authorize]
    public class MascotasController : Controller
    {
        private PruebaContext db = new PruebaContext();
        //private ApplicationDbContext appDb = new ApplicationDbContext();

        // GET: Mascotas
        public ActionResult Index()
        {
            var model = new MascotaViewModel();

            var query = db.Mascotas.Include(m => m.Cliente).Include(m => m.Raza);
            var userId = User.Identity.GetUserId().ToString();

            if (!User.IsInRole("Admin"))
            {
                query = query.Where(x => x.ClienteId == userId);
            }

            model.LoadModelList(query.ToList());
            //var mascotas = db.Mascotas.Include(m => m.Cliente).Include(m => m.Raza).ToList();
            return View(model);
        }

        // GET: Mascotas/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mascota mascota = db.Mascotas.Find(id);
            if (mascota == null)
            {
                return HttpNotFound();
            }

            var model = new MascotaViewModel();
            model.LoadModel(mascota);

            return View(model);
        }

        // GET: Mascotas/Create
        public ActionResult Create()
        {            
            //ViewBag.ClienteId = new SelectList(db.Users, "Id", "Nombre");
            ViewBag.RazaId = new SelectList(db.Razas, "Id", "Nombre");
            ViewBag.ClienteId = User.Identity.Name;
            return View();
        }

        // POST: Mascotas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Apodo,Fecha_nac,Sexo,Foto,ClienteId,RazaId")] MascotaViewModel mascotaVm)
        {
            if (ModelState.IsValid)
            {
                var mascota = mascotaVm.BindItem();
                mascota.Id = Guid.NewGuid();
                db.Mascotas.Add(mascota);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.ClienteId = new SelectList(db.Users, "Id", "Nombre", mascotaVm.ClienteId);
            ViewBag.RazaId = new SelectList(db.Razas, "Id", "Nombre", mascotaVm.RazaId);
            ViewBag.ClienteId = User.Identity.Name;

            return View(mascotaVm);
        }

        // GET: Mascotas/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mascota mascota = db.Mascotas.Find(id);
            if (mascota == null)
            {
                return HttpNotFound();
            }

            var model = new MascotaViewModel();
            model.LoadModel(mascota);

            ViewBag.ClienteId = new SelectList(db.Users, "Id", "Nombre", mascota.ClienteId);
            ViewBag.RazaId = new SelectList(db.Razas, "Id", "Nombre", mascota.RazaId);
            return View(model);
        }

        // POST: Mascotas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Apodo,Fecha_nac,Sexo,Foto,ClienteId,RazaId")] MascotaViewModel mascotaVm)
        {
            if (ModelState.IsValid)
            {
                var mascota = mascotaVm.BindItem();
                db.Entry(mascota).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(db.Users, "Id", "Nombre", mascotaVm.ClienteId);
            ViewBag.RazaId = new SelectList(db.Razas, "Id", "Nombre", mascotaVm.RazaId);
            return View(mascotaVm);
        }

        // GET: Mascotas/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mascota mascota = db.Mascotas.Find(id);
            if (mascota == null)
            {
                return HttpNotFound();
            }

            var model = new MascotaViewModel();
            model.LoadModel(mascota);

            return View(model);
        }

        // POST: Mascotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Mascota mascota = db.Mascotas.Find(id);
            db.Mascotas.Remove(mascota);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult ListAdmin(int page = 1, string sort = "Nombre", string sortdir = "asc", string tiendaid = "")
        {
            int pageSize = 5;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetMascotas(tiendaid, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            //ViewBag.search = search;

            var model = new HomeClientViewModel();
            model.MascotasModel.LoadModelList(data);

            if (sort != null)
            {
                if (sortdir.ToLower() == "asc")
                    model.MascotasModel.ToGrid = model.MascotasModel.ToGrid.OrderBy(x => GetPropertyValue(x, sort)).Skip(skip).Take(pageSize).ToList();
                else
                    model.MascotasModel.ToGrid = model.MascotasModel.ToGrid.OrderByDescending(x => GetPropertyValue(x, sort)).Skip(skip).Take(pageSize).ToList();
            }

            ViewBag.TiendaId = new SelectList(db.Tiendas, "Id", "Nombre", tiendaid);

            return View(model);
        }

        private static object GetPropertyValue(object src, string propName)
        {
            if (src == null) throw new ArgumentException("Value cannot be null.", "src");
            if (propName == null) throw new ArgumentException("Value cannot be null.", "propName");

            if (propName.Contains("."))//complex type nested
            {
                var temp = propName.Split(new char[] { '.' }, 2);
                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
            }
            else
            {
                var prop = src.GetType().GetProperty(propName);
                return prop != null ? prop.GetValue(src, null) : null;
            }
        }

        private List<Mascota> GetMascotas(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            var v = (from a in db.Mascotas
                     where
                         (a.Cliente.Tienda.Id.ToString().Contains(search))
                     select a
                            ).Include(m => m.Cliente).Include(m => m.Raza);

            totalRecord = v.Count();
            return v.ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                //appDb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
