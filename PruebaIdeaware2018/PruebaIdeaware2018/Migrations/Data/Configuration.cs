namespace PruebaIdeaware2018.Migrations.Data
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using PruebaIdeaware2018.Data;
    using PruebaIdeaware2018.Models;
    using PruebaIdeaware2018.Utils;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PruebaIdeaware2018.Domain.Context.PruebaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Data";
        }

        protected override void Seed(PruebaIdeaware2018.Domain.Context.PruebaContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.Ciudades.AddOrUpdate(x => x.Codigo, TestData.getCiudades().ToArray());
            context.TipoTareas.AddOrUpdate(x => x.Codigo, TestData.getTipoTareas().ToArray());
            context.Razas.AddOrUpdate( x => x.Codigo, TestData.getRazas().ToArray());
            context.SaveChanges();

            context.Tiendas.AddOrUpdate(x => x.Codigo, TestData.getTiendas(context).ToArray());
            context.Tareas.AddOrUpdate(x => x.Codigo, TestData.getTareas(context).ToArray());
            context.SaveChanges();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var roleCreated = false;

            if (!roleManager.RoleExists("Admin"))
                roleManager.Create(new IdentityRole("Admin"));

            if (!roleManager.RoleExists("Invitado"))
            {
                roleManager.Create(new IdentityRole("Invitado"));
                roleCreated = true;
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if(userManager.FindByCedulaAsync(1234567) == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "1234567",
                    Cedula = 1234567,
                    Nombre = "Pepe",
                    Apellido = "Grillo",
                    Email = "pgrillo@correo.net",
                    Direccion = "calle 10 # 10 - 10",
                    Fecha_nac = new DateTime(1970, 1, 19),
                    Sexo = "M",
                    PhoneNumber = "3008549876",
                    TiendaId = context.Tiendas.FirstOrDefault(x => x.Codigo == "AZ01").Id,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = userManager.Create(user, "Ideaware");
                if(result.Succeeded)
                {
                    userManager.AddToRole(userManager.FindByCedulaAsync(1234567).Id, "Invitado");
                }
            }else if(roleCreated)
            {
                userManager.AddToRole(userManager.FindByCedulaAsync(1234567).Id, "Invitado");
            }

            //context.Roles.AddOrUpdate(x => x.Name, new IdentityRole("Admin"));
            //context.Roles.AddOrUpdate(x => x.Name, new IdentityRole("Invitado"));
            //context.Users.AddOrUpdate(x => x.Cedula, TestData.getClientes(context).ToArray());

            context.SaveChanges();

            context.Mascotas.AddOrUpdate(x => new { x.Nombre, x.Apodo }, TestData.getMascotas(context).ToArray());
            context.SaveChanges();

            context.TareasMascotas.AddOrUpdate(x => new { x.MascotaId, x.TareaId }, TestData.getTareasMascotas(context).ToArray());
            context.SaveChanges();
            
        }
    }
}
