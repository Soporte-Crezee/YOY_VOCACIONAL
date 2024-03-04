using POV.CentroEducativo.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Administracion.BO
{
    public class CompraPremium
    {
        public int? CompraPremiumID { get; set; }
        public long? TutorID { get; set; }
        public long? AlumnoID { get; set; }
        public int? PaquetePremiumID { get; set; }
        public double? CostoCompra { get; set; }
        public DateTime? FechaCompra { get; set; }
        public string CodigoCompra { get; set; }
        public Guid? CodigoPaquete { get; set; }

        public Tutor Tutor { get; set; }
        public Alumno Alumno { get; set; }
        public virtual PaquetePremium PaquetePremium { get; set; }
    }
}
