using Microsoft.AspNet.Identity;
using PruebaIdeaware2018.Domain;
using PruebaIdeaware2018.Domain.Context;
using PruebaIdeaware2018.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebaIdeaware2018.Data
{
    public class TestData
    {
        public static List<Ciudad> getCiudades()
        {
            return new List<Ciudad>()
            {
                new Ciudad { Codigo = "BAQ", Nombre = "Barranquilla"},
                new Ciudad { Codigo = "BOG", Nombre = "Bogotá"},
                new Ciudad { Codigo = "CAL", Nombre = "Cali"},
                new Ciudad { Codigo = "CTG", Nombre = "Cartagena"},
                new Ciudad { Codigo = "MED", Nombre = "Medellín"}
            };
        }

        public static List<Tienda> getTiendas(PruebaContext context)
        {
            return new List<Tienda>()
            {
                new Tienda { Codigo = "AZ01", Nombre = "Mi mascota feliz", CiudadId = context.Ciudades.FirstOrDefault( x => x.Codigo == "BOG").Id }
            };
        }

        public static List<ApplicationUser> getClientes(PruebaContext context)
        {
            var hash = new PasswordHasher();
            return new List<ApplicationUser>()
            {
                new ApplicationUser { UserName = "1234567", Cedula = 1234567, PasswordHash = hash.HashPassword("Ideaware"), Nombre = "Pepe", Apellido = "Grillo", Email = "pgrillo@correo.net", Direccion = "calle 10 # 10 - 10", Fecha_nac = new DateTime(1970,1,19), Sexo = "M", PhoneNumber = "3008549876", TiendaId = context.Tiendas.FirstOrDefault( x => x.Codigo == "AZ01").Id, SecurityStamp = Guid.NewGuid().ToString() }
            };
        }

        public static List<TipoTarea> getTipoTareas()
        {
            return new List<TipoTarea>()
            {
                new TipoTarea { Nombre = "Campo", Codigo = "CMP"},
                new TipoTarea { Nombre = "Casa", Codigo = "CAS"}
            };
        }

        public static List<Tarea> getTareas(PruebaContext context)
        {
            return new List<Tarea>()
            {
                new Tarea { Codigo = "SLP", Nombre = "Salida al parque", Costo = 1000, TipoId = context.TipoTareas.FirstOrDefault( x => x.Codigo == "CMP").Id },
                new Tarea { Codigo = "ENT", Nombre = "Entrenamiento", Costo = 5000, TipoId = context.TipoTareas.FirstOrDefault( x => x.Codigo == "CMP").Id },
                new Tarea { Codigo = "VCN", Nombre = "Vacunas", Costo = 10000, TipoId = context.TipoTareas.FirstOrDefault( x => x.Codigo == "CMP").Id },
                new Tarea { Codigo = "IGN", Nombre = "Inspección general", Costo = 8000, TipoId = context.TipoTareas.FirstOrDefault( x => x.Codigo == "CAS").Id },
                new Tarea { Codigo = "ALM", Nombre = "Alimentación", Costo = 12500, TipoId = context.TipoTareas.FirstOrDefault( x => x.Codigo == "CAS").Id },
            };
        }

        public static List<Raza> getRazas()
        {
            return new List<Raza>()
            {
                new Raza { Codigo = "BDG", Nombre = "Bulldog"},
                new Raza { Codigo = "BGL", Nombre = "Beagle"},
                new Raza { Codigo = "PUG", Nombre = "Pug"},
                new Raza { Codigo = "LRT", Nombre = "Labrador retriever"},
                new Raza { Codigo = "PBL", Nombre = "Pit bull"}
            };
        }

        public static List<Mascota> getMascotas(PruebaContext context)
        {
            return new List<Mascota>()
            {
                new Mascota { Nombre = "Benji", Apodo = "Bento", Fecha_nac = new DateTime(2017,8,23), Sexo = "M", ClienteId = context.Users.First().Id, RazaId = context.Razas.FirstOrDefault( x => x.Codigo == "PUG").Id}
            };
        }

        public static List<TareasMascota> getTareasMascotas(PruebaContext context)
        {
            return new List<TareasMascota>()
            {
                new TareasMascota { Fecha_ejec = new DateTime(2018,09,10), MascotaId = context.Mascotas.First().Id, TareaId = context.Tareas.FirstOrDefault( x => x.Codigo == "ALM").Id}
            };
        }
    }
}