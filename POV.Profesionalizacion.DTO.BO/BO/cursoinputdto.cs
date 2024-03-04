using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Profesionalizacion.DTO.BO
{
   public class cursoinputdto
    {

        public int? cursoid { get; set; }
        public string cursonombre { get; set; }
        public int? cursotemaid { get; set; }
        public short? cursopresencial { get; set; }
        public byte cursoestatus { get; set; }
        //paginación
        public int pagesize { get; set; }
        public int currentpage { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
    }
}
