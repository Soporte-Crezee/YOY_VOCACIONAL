using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Prueba.DTO.BO
{
    public class respuestareactivodto
    {
        public string reactivoid { get; set; }
        public long? respuestareactivoid { get; set; }
        public int? tiempo { get; set; }
        public List<respuestapreguntadto> respuestas { get; set; }
        public string hash { get; set; }
        public int? tipopruebapresentacion { get; set; }
    }
}
