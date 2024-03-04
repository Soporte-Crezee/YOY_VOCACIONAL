using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Profesionalizacion.DTO.BO
{
    public class agrupadorinputdto
    {
        public Int64? agrupadorID { get; set; }
        public Int32 tipoAgrupador { get; set; }
        public Int64? ejeID { get; set; }
        public int? pagesize { get; set; }
        public int? currentpage { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
    }
}
