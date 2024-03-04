using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Prueba.DTO.BO
{
    public class respuestapreguntadto
    {
        public int? preguntaid { get; set; }
        public long? respuestapreguntaid { get; set; }
        public List<opciondto> opciones { get; set; }
        public string textorespuesta { get; set; }
        public decimal? valorespuesta { get; set; }


        public string hash { get; set; }
    }
}
