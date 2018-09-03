using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PruebaIdeaware2018.Domain
{
    public class Clientesasdas : IdentityUser
    {
        public int Cedula { get; set; }
        public string Clave { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public string Correo { get; set; }
        public DateTime Fecha_nac { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Sexo { get; set; }
        public byte[] Foto { get; set; }
        
        public Guid TiendaId { get; set; }
        public virtual Tienda Tienda { get; set; }
        public virtual List<Mascota> Mascotas { get; set; }
    }
}