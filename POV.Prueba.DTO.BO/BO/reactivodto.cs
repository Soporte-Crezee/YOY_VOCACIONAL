using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Prueba.DTO.BO
{
    public class reactivodto
    {
        public string reactivoid { get; set; }
        
        public string texto { get; set; }
        public string imagenurl { get; set; }

        public byte? tipo { get; set; }
        public byte? tipopresentacion { get; set; }
        public byte? tipopruebapresentacion { get; set; }
        public bool? esfinal { get; set; }

        public List<preguntadto> preguntas { get; set; }

        public long? alumnoid { get; set; }
        public long? respuestareactivoid { get; set; }

        public string hash { get; set; }
    }
}
