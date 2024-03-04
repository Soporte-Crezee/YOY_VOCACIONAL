using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class reactivoinputdto
    {
        public int pagesize { get; set; }
        public int currentpage { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
        public string reactivoid { get; set; }
        public string nombrereactivo { get; set; }
        public int? tipocomplejidadid { get; set; }
        public int? areaaplicacionid { get; set; }
    }
}
