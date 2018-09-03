using PruebaIdeaware2018.Domain;
using PruebaIdeaware2018.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace PruebaIdeaware2018.Models
{
    public class MascotaViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Apodo { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [ValidarFechaRango(Menor = true)]
        public DateTime Fecha_nac { get; set; }

        public string Sexo { get; set; }

        public string SexoToDisplay
        {
            get
            {
                return Sexo == "M" ? "Macho" : "Hembra";
            }
            set
            {
                Sexo = value;
            }
        }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase Foto { get; set; }

        public byte[] FotoByte { get; set; }

        [Required]
        public string ClienteId { get; set; }
        public virtual ApplicationUser Cliente { get; set; }
        public Guid RazaId { get; set; }
        public virtual Raza Raza { get; set; }
        public virtual List<TareasMascota> Tareas { get; set; }

        public List<MascotaViewModel> ToGrid { get; set; }

        public void LoadModel(Mascota mascota)
        {
            Id = mascota.Id;
            Apodo = mascota.Apodo;
            Cliente = mascota.Cliente;
            ClienteId = mascota.ClienteId;
            Fecha_nac = mascota.Fecha_nac;
            FotoByte = mascota.Foto;
            Nombre = mascota.Nombre;
            Raza = mascota.Raza;
            RazaId = mascota.RazaId;
            Sexo = mascota.Sexo;
            Tareas = mascota.Tareas;
        }

        public void LoadModelList(List<Mascota> mascotas)
        {
            ToGrid = new List<MascotaViewModel>();

            foreach (var mascota in mascotas)
            {
                var mascotavm = new MascotaViewModel
                {
                    Id = mascota.Id,
                    Apodo = mascota.Apodo,
                    Cliente = mascota.Cliente,
                    ClienteId = mascota.ClienteId,
                    Fecha_nac = mascota.Fecha_nac,
                    //Foto = mascota.Foto,
                    Nombre = mascota.Nombre,
                    Raza = mascota.Raza,
                    RazaId = mascota.RazaId,
                    Sexo = mascota.Sexo,
                    Tareas = mascota.Tareas

                };
                ToGrid.Add(mascotavm);
            }
        }

        public Mascota BindItem()
        {
            var mascota = new Mascota
            {
                Id = this.Id,
                Apodo = this.Apodo,
                Cliente = this.Cliente,
                ClienteId = this.ClienteId,
                Fecha_nac = this.Fecha_nac,
                //Foto = mascota.Foto,
                Nombre = this.Nombre,
                Raza = this.Raza,
                RazaId = this.RazaId,
                Sexo = this.Sexo,
                Tareas = this.Tareas
            };

            if (this.Foto != null && this.Foto.ContentLength > 0)
            {
                using (Stream inputStream = this.Foto.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    mascota.Foto = memoryStream.ToArray();
                }
            }

            return mascota;
        }

        public string FotoSrc
        {
            get
            {
                if (this.FotoByte != null && this.FotoByte.Length > 0)
                {
                    string mimeType = "image/png";
                    string base64 = Convert.ToBase64String(this.FotoByte);
                    return string.Format("data:{0};base64,{1}", mimeType, base64);
                }
                else
                    return "";
            }
        }

        [Display(Name = "Costo total")]
        [DataType(DataType.Currency)]
        public int TotalPay
        {
            get
            {
                if (this.Tareas == null || !this.Tareas.Any())
                    return 0;
                return this.Tareas.Where(x => x.Fecha_ejec.Date >= DateTime.Now.Date).Sum(tarea => tarea.Tarea.Costo);
                /*var toPay = 0;
                foreach (var tarea in this.Tareas)
                {
                    toPay += tarea.Tarea.Costo;
                }
                return toPay;*/
            }
        }

        [Display(Name = "Fecha prox tarea")]
        public string NearDateTask
        {
            get
            {
                if (this.Tareas == null || !this.Tareas.Any(x => x.Fecha_ejec.Date >= DateTime.Now.Date))
                    return "No existen tareas pendiente";
                return this.Tareas.OrderBy(x => x.Fecha_ejec).FirstOrDefault(x => x.Fecha_ejec.Date >= DateTime.Now.Date).Fecha_ejec.ToString("d");
                /*var toPay = 0;
                foreach (var tarea in this.Tareas)
                {
                    toPay += tarea.Tarea.Costo;
                }
                return toPay;*/
            }
        }

        [Display(Name = "Tareas para hoy")]
        public int TodayTasksCount
        {
            get
            {
                if (this.Tareas == null || !this.Tareas.Any())
                    return 0;
                return this.Tareas.Count( x => x.Fecha_ejec.Date == DateTime.Now.Date);
            }
        }

        [Display(Name = "% Tareas resueltas")]
        [DisplayFormat(DataFormatString = "{0:P2}", NullDisplayText = "Sin tareas aún")]
        [Range(0.1, 100)]
        public decimal? PercentageTasksDone
        {
            get
            {
                if (this.Tareas == null || !this.Tareas.Any())
                    return null;
                return (this.Tareas.Count(x => x.Fecha_ejec.Date < DateTime.Now.Date) / this.Tareas.Count());
            }
        }
    }

    public class HomeClientViewModel
    {
        public HomeClientViewModel()
        {
            MascotasModel = new MascotaViewModel();
        }

        public ApplicationUser Cliente { get; set; }
        
        public MascotaViewModel MascotasModel { get; set; }

        [Display(Name = "Total a pagar hoy")]
        [DataType(DataType.Currency)]
        public int TotalPagarHoy { get; set; }

    }

    public class UserViewModel
    {
        public string Id { get; set; }
        public int Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Correo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [ValidarFechaRango(Menor = true)]
        public DateTime? Fecha_nac { get; set; }
        public string Direccion { get; set; }
        public string Sexo { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Foto { get; set; }

        public byte[] FotoByte { get; set; }

        public Guid? TiendaId { get; set; }
        public Tienda Tienda { get; set; }
        public List<Mascota> Mascotas { get; set; }

        public List<Ciudad> Ciudades { get; set; }

        public List<UserViewModel> ToGrid { get; set; }

        public void LoadModel(ApplicationUser user)
        {
            this.Id = user.Id;
            this.Nombre = user.Nombre;
            this.Apellido = user.Apellido;
            this.Fecha_nac = user.Fecha_nac;
            this.Direccion = user.Direccion;
            this.Sexo = user.Sexo;
            this.Correo = user.Email;
            this.Telefono = user.PhoneNumber;
            this.FotoByte = user.Foto;

            this.Tienda = user.Tienda;
            this.Mascotas = user.Mascotas;
        }

        public ApplicationUser BindItem(ApplicationUser user)
        {
            if(user != null)
            {
                user.Nombre = this.Nombre;
                user.Apellido = this.Apellido;
                user.Email = this.Correo;
                user.Fecha_nac = this.Fecha_nac;
                user.Direccion = this.Direccion;
                user.PhoneNumber = this.Telefono;
                user.Sexo = this.Sexo;
                user.TiendaId = this.TiendaId;

                if (this.Foto != null && this.Foto.ContentLength > 0)
                {
                    using (Stream inputStream = this.Foto.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        user.Foto = memoryStream.ToArray();
                    }
                }

                return user;
            }

            var appuser = new ApplicationUser
            {
                Id = this.Id,
                Nombre = this.Nombre,
                Apellido = this.Apellido,
                Email = this.Correo,
                Fecha_nac = this.Fecha_nac,
                Direccion = this.Direccion,
                PhoneNumber = this.Telefono,
                Sexo = this.Sexo
            };
            
            if (this.Foto != null && this.Foto.ContentLength > 0)
            {
                using (Stream inputStream = this.Foto.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    appuser.Foto = memoryStream.ToArray();
                }
            }

            return appuser;
        }

        public string FotoSrc
        {
            get
            {
                if (this.FotoByte != null && this.FotoByte.Length > 0)
                {
                    string mimeType = "image/png";
                    string base64 = Convert.ToBase64String(this.FotoByte);
                    return string.Format("data:{0};base64,{1}", mimeType, base64);
                }
                else
                    return "";
            }
        }

        public void LoadModelList(List<ApplicationUser> users)
        {
            ToGrid = new List<UserViewModel>();

            foreach (var user in users)
            {
                var uservm = new UserViewModel()
                {
                    Id = user.Id,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Correo = user.Email,
                    Fecha_nac = user.Fecha_nac,
                    Direccion = user.Direccion,
                    Telefono = user.PhoneNumber,
                    TiendaId = user.TiendaId,
                    Tienda = user.Tienda,
                    Cedula = user.Cedula                     
                };
                ToGrid.Add(uservm);
            }
        }

    }

}