using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using PruebaIdeaware2018.Domain;
using PruebaIdeaware2018.Domain.Context;

namespace PruebaIdeaware2018.Controllers.API
{
    public class WebApiController : ApiController
    {
        private PruebaContext db = new PruebaContext();

        // GET: api/WebApi
        public IQueryable<TareaApiResponse> GetTareasMascotas(string nombre = "", string tipo = "", int pageSize = 5, int pageIndex = 1)
        {
            var query = GetTareasMascotas();

            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(x => x.Nombre.ToLower().Contains(nombre.ToLower()));
            }

            if (!string.IsNullOrEmpty(tipo))
            {
                query = query.Where(x => x.Tipo.ToLower().Contains(tipo.ToLower()));
            }

            return paginarTareasMascotas(query, pageSize, pageIndex - 1);
        }

        private IQueryable<TareaApiResponse> GetTareasMascotas()
        {
            var query = db.TareasMascotas.Select(x => new TareaApiResponse()
            {
                Tipo = x.Tarea.Tipo.Nombre,
                Nombre = x.Tarea.Nombre,
                Cliente = x.Mascota.Cliente.Nombre + " " + x.Mascota.Cliente.Apellido,
                MascotaEdad = DateTime.Now.Month < x.Mascota.Fecha_nac.Month ? (DateTime.Now.Year - x.Mascota.Fecha_nac.Year - 1).ToString() : DateTime.Now.Month == x.Mascota.Fecha_nac.Month ? DateTime.Now.Day >= x.Mascota.Fecha_nac.Day ? (DateTime.Now.Year - x.Mascota.Fecha_nac.Year).ToString() : (DateTime.Now.Year - x.Mascota.Fecha_nac.Year - 1).ToString() : (DateTime.Now.Year - x.Mascota.Fecha_nac.Year).ToString(),
                Raza = x.Mascota.Raza.Nombre,
                Fecha = x.Fecha_ejec,
                Mascota = x.Mascota.Nombre
            });

            return query;
        }

        private IQueryable<TareaApiResponse> paginarTareasMascotas(IQueryable<TareaApiResponse> query, int pageSize, int pageIndex)
        {
            if (pageSize > 5) pageSize = 5;

            query = query.OrderBy(x => x.Fecha).Skip(pageSize * pageIndex).Take(pageSize);

            return query;
        }

        // GET: api/WebApi/5
        [ResponseType(typeof(TareasMascota))]
        public IHttpActionResult GetTareasMascota(Guid id)
        {
            TareasMascota tareasMascota = db.TareasMascotas.Find(id);
            if (tareasMascota == null)
            {
                return NotFound();
            }

            return Ok(tareasMascota);
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

    public class TareaApiResponse
    {
        public string Tipo { get; set; }
        public string Nombre { get; set; }
        public string Cliente { get; set; }
        public string MascotaEdad { get; set; }
        public string Mascota { get; set; }
        public string Raza { get; set; }
        public DateTime Fecha { get; set; }

    }
}