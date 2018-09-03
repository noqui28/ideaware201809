using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PruebaIdeaware2018.Domain
{
    public class Tarea
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }

        [DataType(DataType.Currency)]
        public int Costo { get; set; }

        public Guid TipoId { get; set; }
        public virtual TipoTarea Tipo { get; set; }
        public virtual List<TareasMascota> TareaMascota { get; set; }
    }
}