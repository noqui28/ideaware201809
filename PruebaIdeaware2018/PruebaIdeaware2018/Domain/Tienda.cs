using PruebaIdeaware2018.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PruebaIdeaware2018.Domain
{
    public class Tienda
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }

        public Guid CiudadId { get; set; }
        public virtual Ciudad Ciudad { get; set; }
        public virtual List<ApplicationUser> Clientes { get; set; }

    }
}