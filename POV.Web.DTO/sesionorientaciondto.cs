using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class sesionorientaciondto
    {
        public int? sesionorientacionid { get; set; }
        public string inicio { get; set; }
        public string fin { get; set; }
        public string fecha { get; set; }
        public int? asistenciaaspirante { get; set; }
        public int? asistenciaorientador { get; set; }
        public int? cantidadhoras { get; set; }
        public long? alumnoid { get; set; }
        public int? docenteid { get; set; }
        public int? estatussesion { get; set; }
        public int? encuestacontestada { get; set; }
        public string horafinalizada { get; set; }

        public string Error { get; set; }
        public string Success { get; set; }
    }
}
