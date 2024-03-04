using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.CentroEducativo.BO
{
    public class EncuestaSatisfaccion
    {
        public int? EncuestaID { get; set; }

        public string PreguntaUno { get; set; }
        public string RespuestaUno { get; set; }

        public string PreguntaDos { get; set; }
        public string RespuestaDos { get; set; }

        public string PreguntaTres { get; set; }
        public string RespuestaTres { get; set; }

        public string PreguntaCuatro { get; set; }
        public string RespuestaCuatro { get; set; }

        public string PreguntaCinco { get; set; }
        public string RespuestaCinco { get; set; }

        public int? DocenteID { get; set; }
        public long? AlumnoID { get; set; }
        public int? SesionOrientacionID { get; set; }
    }
}
