using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class centrocomputodto
    {
        public int? centrocomputoid { get; set; }
        public bool? tienecentro { get; set; }
        public bool? tieneinternet { get; set; }
        public decimal? anchobanda { get; set; }
        public string proveedor { get; set; }
        public string tipocontrato { get; set; }
        public string responsable { get; set; }
        public long? telefono { get; set; }
        public int? numpcs { get; set; }
     
           
    }
}
