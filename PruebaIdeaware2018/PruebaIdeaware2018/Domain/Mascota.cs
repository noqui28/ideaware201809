using PruebaIdeaware2018.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaIdeaware2018.Domain
{
    public class Mascota
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Apodo { get; set; }
        public DateTime Fecha_nac { get; set; }
        public string Sexo { get; set; }
        [DataType(DataType.Upload)]
        public byte[] Foto { get; set; }

        [Required]
        public string ClienteId { get; set; }
        public virtual ApplicationUser Cliente { get; set; }
        public Guid RazaId { get; set; }
        public virtual Raza Raza { get; set; }
        public virtual List<TareasMascota> Tareas { get; set; }
    }
}