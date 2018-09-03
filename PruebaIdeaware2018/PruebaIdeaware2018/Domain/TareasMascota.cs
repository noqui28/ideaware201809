using PruebaIdeaware2018.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PruebaIdeaware2018.Domain
{
    public class TareasMascota
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid TareaId { get; set; }
        public virtual Tarea Tarea { get; set; }
        public Guid MascotaId { get; set; }
        public virtual Mascota Mascota { get; set; }

        [Display(Name = "Fecha de realización")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [ValidarFechaRango(Menor = false)]
        public DateTime Fecha_ejec { get; set; }
    }
}