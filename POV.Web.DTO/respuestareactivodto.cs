using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class respuestareactivodto
    {
        public string reactivoid { get; set; }
        public List<respuestapreguntadto> preguntas { get; set; }


        public string retroalimentacion { get; set; }
        public bool? fallocompleto { get; set; }
        public bool? rendersugerir { get; set; }

        public bool? success { get; set; }
        public string error { get; set; }

    }
}
