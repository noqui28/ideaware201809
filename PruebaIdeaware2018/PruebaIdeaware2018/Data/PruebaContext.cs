namespace PruebaIdeaware2018.Domain.Context
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using PruebaIdeaware2018.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class PruebaContext : IdentityDbContext<ApplicationUser>
    {
        // El contexto se ha configurado para usar una cadena de conexión 'PruebaContext' del archivo 
        // de configuración de la aplicación (App.config o Web.config). De forma predeterminada, 
        // esta cadena de conexión tiene como destino la base de datos 'PruebaIdeaware2018.Domain.Context.PruebaContext' de la instancia LocalDb. 
        // 
        // Si desea tener como destino una base de datos y/o un proveedor de base de datos diferente, 
        // modifique la cadena de conexión 'PruebaContext'  en el archivo de configuración de la aplicación.
        public PruebaContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public static PruebaContext Create()
        {
            return new PruebaContext();
        }

        // Agregue un DbSet para cada tipo de entidad que desee incluir en el modelo. Para obtener más información 
        // sobre cómo configurar y usar un modelo Code First, vea http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<Ciudad> Ciudades { get; set; }

        public virtual DbSet<Tienda> Tiendas { get; set; }

        public virtual DbSet<Mascota> Mascotas { get; set; }

        public virtual DbSet<TareasMascota> TareasMascotas { get; set; }

        public virtual DbSet<Tarea> Tareas { get; set; }

        public virtual DbSet<TipoTarea> TipoTareas { get; set; }

        public virtual DbSet<Raza> Razas { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}