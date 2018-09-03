using PruebaIdeaware2018.Domain;
using PruebaIdeaware2018.Domain.Context;
using PruebaIdeaware2018.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;
using Microsoft.AspNet.Identity;
using System;
using System.Reflection;

namespace PruebaIdeaware2018.Controllers
{
    public class HomeController : Controller
    {
        private PruebaContext db = new PruebaContext();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("HomeClient", "Home");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult HomeClient(int page = 1, string sort = "Nombre", string sortdir = "asc", string search = "", string searchoperator = "igual", int numtareas = -1)
        {
            ViewBag.Message = "Bienvenido estimado cliente.";

            int pageSize = 5;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetMascotas(search, numtareas, searchoperator, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            ViewBag.numtareas = numtareas < 0 ? "" : numtareas.ToString();

            var model = new HomeClientViewModel();
            model.Cliente = db.Users.First();
            model.MascotasModel.LoadModelList(data);

            if (sort != null)
            {
                if (sortdir.ToLower() == "asc")
                    model.MascotasModel.ToGrid = model.MascotasModel.ToGrid.OrderBy(x => GetPropertyValue(x, sort)).Skip(skip).Take(pageSize).ToList();
                else
                    model.MascotasModel.ToGrid = model.MascotasModel.ToGrid.OrderByDescending(x => GetPropertyValue(x, sort)).Skip(skip).Take(pageSize).ToList();
            }
            model.TotalPagarHoy = (from tarea in db.TareasMascotas where tarea.Fecha_ejec == DateTime.Today select tarea.Tarea.Costo).Concat(new[] { 0 }).Sum(x => x);

            return View(model);
        }

        public static object GetPropertyValue(object src, string propName)
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

        private List<Mascota> GetMascotas(string search, int numtareas, string optareas, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            var userID = HttpContext.User.Identity.GetUserId();
            //db.Mascotas.Where( x => x.Nombre.Contains(search) || x.Apodo.Contains(search))
            var v = (from a in db.Mascotas
                        where
                            a.ClienteId == userID &&
                            (a.Nombre.Contains(search) || a.Apodo.Contains(search))
                        select a
                            ).Include(m => m.Cliente).Include(m => m.Raza);
                
            if (numtareas >= 0)
            {

                switch (optareas)
                {
                    case "igual":
                        v = v.Where(x => x.Tareas.Count == numtareas);
                        break;
                    case "mayor":
                        v = v.Where(x => x.Tareas.Count > numtareas);
                        break;
                    case "menor":
                        v = v.Where(x => x.Tareas.Count < numtareas);
                        break;
                    case "mayorigual":
                        v = v.Where(x => x.Tareas.Count >= numtareas);
                        break;
                    case "menorigual":
                        v = v.Where(x => x.Tareas.Count <= numtareas);
                        break;
                }
            }

            totalRecord = v.Count();

            var result = v.ToList();
            /*
                v = v.OrderBy(sort + " " + sortdir);
                if (pageSize > 0)
                {
                    v = v.Skip(skip).Take(pageSize);
                }
                return v.ToList();*/
            return result;
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