using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class respuestaencuestadto
    {
        public int? encuestaid { get; set; }

        public string preguntauno { get; set; }
        public string respuestauno{ get; set; }

        public string preguntados { get; set; }
        public string respuestados { get; set; }

        public string preguntatres { get; set; }
        public string respuestatres { get; set; }

        public string preguntacuatro { get; set; }
        public string respuestacuatro { get; set; }

        public string preguntacinco { get; set; }
        public string respuestacinco { get; set; }

        public int? docenteid { get; set; }
        public long? alumnoid { get; set; }
        public int? sesionorientacionid { get; set; }

        public string Error { get; set; }
        public string Success { get; set; }
    }
}
