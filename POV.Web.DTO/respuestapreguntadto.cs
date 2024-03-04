using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class respuestapreguntadto
    {
        public int? preguntaid { get; set; }
        public string textoabierta { get; set; }
        public int? opcionseleccionadaid { get; set; }
    }
}
