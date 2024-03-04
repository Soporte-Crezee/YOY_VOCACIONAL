using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class configcalendardto
    {
        public long? configcalendarid { get; set; }
        public int? usuarioid { get; set; }
        public string diaslaborales { get; set; }
        public string iniciotrabajo { get; set; }
        public string fintrabajo { get; set; }
        public string iniciodescanso { get; set; }
        public string findescanso { get; set; }

        public string Error { get; set; }
        public string Success { get; set; }
    }
}
