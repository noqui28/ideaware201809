using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PruebaIdeaware2018.Domain
{
    public class TipoTarea
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }

        public List<Tarea> Tareas { get; set; }
    }
}