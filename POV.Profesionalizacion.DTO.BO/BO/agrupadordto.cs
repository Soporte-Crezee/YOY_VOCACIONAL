using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Profesionalizacion.DTO.BO
{
    public class agrupadordto
    {
        public Int64? agrupadorid { get; set; }
        public Int32 tipoAgrupador { get; set; }
        public Int64? ejeid { get; set; }
        public string nombre { get; set; }
        public string competencias { get; set; }
        public string aprendizajes { get; set; }
        public List<contenidodigitaldto> contenidosdigitales { get; set; }
        public int? pagesize { get; set; }
        public int? currentpage { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
    }
}
