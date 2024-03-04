using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Prueba.DTO.BO
{
    public class opciondto
    {
        public int? opcionid { get; set; }
        public string texto { get; set; }
        public string imagenurl { get; set; }
        public int? preguntaid { get; set; }
        public byte? tipopresentacion { get; set; }
        public int? clasificadorid { get; set; }
    }
}
