using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Reactivos.BO
{
    public class PlantillaKuder 
    {
        public int? PlantillaKuderID { get; set; }
        public string Clave { get; set; }
        public string Plantilla { get; set; }
        public int? ClasificadorID { get; set; }
        public string RespuestaOpcion { get; set; }
        public string GradoInteres { get; set; }
        public int? Grupo { get; set; }
    }
}
