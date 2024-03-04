using POV.Localizacion.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.CentroEducativo.BO
{
    public class Tutor : ICloneable
    {
        public long? TutorID { get; set; }
        /// <summary>
        /// Nombre del tutor
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Primer Apellido del tutor
        /// </summary>
        public string PrimerApellido { get; set; }
        /// <summary>
        /// Segundo Apellido del tutor
        /// </summary>
        public string SegundoApellido { get; set; }
        /// <summary>
        /// Fecha de nacimiento del tutor
        /// </summary>
        public DateTime? FechaNacimiento { get; set; }
        /// <summary>
        /// Direccion del tutor
        /// </summary>
        public string Direccion { get; set; }
        /// <summary>
        /// Estatus
        /// </summary>
        public bool? Estatus { get; set; }
        /// <summary>
        /// Fecha en que se realizo el registro
        /// </summary>
        public DateTime? FechaRegistro { get; set; }
        /// <summary>
        /// Sexo del tutor
        /// </summary>
        public bool? Sexo { get; set; }
        /// <summary>
        /// ¿Correo confirmado?
        /// </summary>
        public bool? CorreoConfirmado { get; set; }
        /// <summary>
        /// ¿Datos completos?
        /// </summary>
        public bool? DatosCompletos { get; set; }
        /// <summary>
        /// Codigo del tutor
        /// </summary>
        public string Codigo { get; set; }
        /// <summary>
        /// Ubicacion del tutor
        /// </summary>
        /// 
        public long? UbicacionID { get; set; }
        public Ubicacion Ubicacion { get; set; }

        public virtual List<TutorAlumno> Alumnos { get; set; }

        public bool? EstatusIdentificacion { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Devuelve la edad del Tutor expresada en años calculada con la fecha actual
        /// </summary>
        /// <returns>int edadEnAnios</returns>
        public int EdadEnAnios()
        {
            DateTime dFechaActual = System.DateTime.Today;
            return EdadEnAnios(dFechaActual);
        }

        /// <summary>
        /// Devuelve la edad del Tutor expresada en años calculada con una fecha actual específica
        /// </summary>
        /// <param name="dFechaActual">fecha hasta la que se requiere calcular la edad</param>
        /// <returns>int edadEnAnios</returns>
        public int EdadEnAnios(DateTime dFechaActual)
        {
            DateTime dFechaNac = (DateTime)this.FechaNacimiento;
            int nAnios = dFechaActual.Year - dFechaNac.Year;
            DateTime dFechaComp = dFechaNac.Date.AddYears(nAnios);               // sólo la parte de fecha en dFechaNac
            return dFechaActual.Date.CompareTo(dFechaComp) < 0 ? --nAnios : nAnios;     // sólo la parte de fecha en dFechaActual
        }

        public double? Credito { get; set; }
        public double? CreditoUsado { get; set; }
        public double? Saldo { get; set; }
        public Int16? Parentesco { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string Curp { get; set; }

        public bool? NotificacionPago { get; set; }
    }
}
