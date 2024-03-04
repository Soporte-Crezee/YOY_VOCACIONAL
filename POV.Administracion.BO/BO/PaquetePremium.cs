using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Administracion.BO
{
    public class PaquetePremium
    {
        public int? PaquetePremiumID { get; set; }
        public string Nombre { get; set; }
        public double? CostoPaquete { get; set; }
        public long? HorasTutor { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool? Estatus { get; set; }

        public virtual List<CompraPremium> CompraPremium { get; set; }
    }
}
