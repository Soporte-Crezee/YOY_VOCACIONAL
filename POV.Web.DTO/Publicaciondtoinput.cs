using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class publicaciondtoinput
    {
        public int pagesize { get; set; }
        public int currentpage { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
        public long socialhubid { get; set; }
        public long usuariosocialid { get; set; }
        public int? estatus { get; set; }
    }
}
