using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class opcionrespuestadto
    {
        public int? opcionid { get; set; }
        public int? preguntaid { get; set; }
        public string texto { get; set; }
        public string imagen { get; set; }
        public bool? predeterminado { get; set; }
        public bool? check { get; set; }

        public bool? success { get; set; }
    }
}
