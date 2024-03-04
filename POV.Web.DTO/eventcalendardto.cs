using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class eventcalendardto
    {
        public long? eventcalendarid { get; set; }
        public int? usuarioid { get; set; }
        public string asunto { get; set; }
        public string fecha { get; set; }
        public string hrsinicio { get; set; }
        public string hrsfin { get; set; }

        //alumno
        public long? alumnoid { get; set; }
        public string nombrecompletoalumno { get; set; }

        public int? cantidadhoras { get; set; }

        public string Error { get; set; }
        public string Success { get; set; }
    }
}
