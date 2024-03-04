using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class preguntadto
    {
        public int? preguntaid {get;set;}
        public string reactivoid { get; set; }
        public int? orden { get; set; }
        public string textopregunta { get; set; }
        public string plantilla { get; set; }
        public short? tipoplantilla { get; set; }
        public string fecharegistro { get; set; }
        public int? plantillaid { get; set; }
        public DateTime? fecharegistroD { get; set; }
        public List<opcionrespuestadto> opciones { get; set; }
        public int? maximocarateres { get; set; }

        public bool? success { get; set; }
        public string error { get; set; }
    }
}
