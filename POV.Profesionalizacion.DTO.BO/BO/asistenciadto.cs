using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Profesionalizacion.DTO.BO
{
    public class asistenciadto
    {
        public long? asistenciaid { get; set; }
        public string nombre { get; set; }
        public string tema { get; set; }

        //Variables de control.
        public bool? success { get; set; }
        public string errors { get; set; }
    }
}
