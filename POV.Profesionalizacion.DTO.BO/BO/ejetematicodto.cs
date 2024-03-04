using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Profesionalizacion.DTO.BO
{
    public class ejetematicodto
    {
        public long? ejetematicoid { get; set; }
        public string nombreejetematico { get; set; }
        public string nombrearea { get; set; }
        public string nombremateria { get; set; }
        public situacionaprendizajeoutputdto situacionoutputdto { get; set; }
    }
}
