using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PruebaIdeaware2018.Utils
{
    public class ValidarFechaRango : ValidationAttribute
    {        
        public Boolean Menor { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var fecha = (DateTime)value;
            return fecha == null ? new ValidationResult("Fecha invalida") : Menor ? DateTime.Now.Date >= fecha ? ValidationResult.Success : new ValidationResult("Fecha invalida") : DateTime.Now.Date <= fecha ? ValidationResult.Success : new ValidationResult("Fecha invalida");            
        }

    }
}