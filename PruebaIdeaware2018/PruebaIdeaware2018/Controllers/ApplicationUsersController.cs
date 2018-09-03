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
using PruebaIdeaware2018.Models;

namespace PruebaIdeaware2018.Controllers
{
    [Authorize]
    public class ApplicationUsersController : Controller
    {
        private PruebaContext db = new PruebaContext();

        // GET: ApplicationUsers
        public ActionResult Index()
        {
            var applicationUsers = db.Users.Include(a => a.Tienda);
            var model = new UserViewModel();
            model.LoadModelList(applicationUsers.ToList());
            return View(model);
        }

        // GET: ApplicationUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            var model = new UserViewModel();
            model.LoadModel(applicationUser);

            return View(model);
        }

        // GET: ApplicationUsers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.TiendaId = new SelectList(db.Tiendas, "Id", "Nombre");
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Cedula,Nombre,Apellido,Fecha_nac,Direccion,Sexo,Foto,TiendaId,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(applicationUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TiendaId = new SelectList(db.Tiendas, "Id", "Nombre", applicationUser.TiendaId);
            return View(applicationUser);
        }

        [HttpPost]
        public ActionResult GetTiendas(string ciudadId)
        {
            Guid id;
            Guid.TryParse(ciudadId, out id);
            var tiendas = db.Tiendas.Where(x => x.CiudadId == id);
            SelectList listTiendas = new SelectList(tiendas, "Id", "Nombre");
            return Json(listTiendas);
        }

        // GET: ApplicationUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            var model = new UserViewModel();
            model.LoadModel(applicationUser);

            model.Ciudades = db.Ciudades.ToList();
            if (applicationUser.Tienda != null)
            {
                ViewBag.TiendaId = new SelectList(db.Tiendas.Where(x => x.CiudadId == applicationUser.Tienda.CiudadId), "Id", "Nombre", applicationUser.TiendaId);
            }
            else
            {
                ViewBag.TiendaId = new SelectList(new List<Tienda>() , "Id", "Codigo");
            }

            return View(model);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cedula,Nombre,Apellido,Fecha_nac,Direccion,Telefono,Sexo,Foto,TiendaId,Correo")] UserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(x => x.Id == userModel.Id);
                var applicationUser = userModel.BindItem(user);
                applicationUser.EmailConfirmed = true;
                db.Entry(applicationUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            userModel.LoadModel(db.Users.Find(userModel.Id));

            userModel.Ciudades = db.Ciudades.ToList();
            if (userModel.Tienda != null)
            {
                ViewBag.TiendaId = new SelectList(db.Tiendas.Where(x => x.CiudadId == userModel.Tienda.CiudadId), "Id", "Nombre", userModel.TiendaId);
            }
            else
            {
                ViewBag.TiendaId = new SelectList(new List<Tienda>(), "Id", "Codigo");
            }            
            return View(userModel);
        }

        // GET: ApplicationUsers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
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
