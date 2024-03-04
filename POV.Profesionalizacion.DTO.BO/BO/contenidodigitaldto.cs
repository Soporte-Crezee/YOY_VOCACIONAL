using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.ContenidosDigital.DTO.BO;

namespace POV.Profesionalizacion.DTO.BO
{
    public class contenidodigitaldto
    {
        public long? asistenciaid { get; set; }
        public long? contenidodigitalid { get; set; }
        public string tipodocumento { get; set; }
        public string nombrecontenidodigital { get; set; }
        public int currentpage { get; set; }
        public List<contenidodigitaldto> contenidos { get; set; }
    }
}
