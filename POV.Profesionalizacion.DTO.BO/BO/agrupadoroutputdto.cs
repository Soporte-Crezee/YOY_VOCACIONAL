using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Profesionalizacion.DTO.BO
{
    public class agrupadoroutputdto
    {
        public Int64? agrupadorID { get; set; }
        public Int32 tipoAgrupador { get; set; }
        public List<agrupadordto> agrupadores { get; set; }
    }
}
