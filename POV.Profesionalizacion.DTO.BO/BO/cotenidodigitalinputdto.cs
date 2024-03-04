using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Profesionalizacion.DTO.BO
{
    public class cotenidodigitalinputdto
    {
        public long? asistenciaid { get; set; }
        public long? contenidodigitalid { get; set; }
        public long? temaid { get; set; }
        public long? tipodocumentoid { get; set; }

        //Filtros de Búsqueda.
        public string nombre { get; set; }
        public string tema { get; set; }
        public string tipodocumento { get; set; }

        //paginación
        public int? pagesize { get; set; }
        public int? currentpage { get; set; }
        public string sort { get; set; }
        public string order { get; set; }

        //variables de control
        public bool? success { get; set; }
        public string errors { get; set; }
    }
}
