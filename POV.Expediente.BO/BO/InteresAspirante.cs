using POV.Modelo.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Expediente.BO
{
    public class InteresAspirante
    {
        public int? InteresID { get; set; }
        public string NombreInteres { get; set; }
        public string Descripcion { get; set; }
        public Clasificador clasificador { get; set; }
        public bool? Activo { get; set; }
    }
}
