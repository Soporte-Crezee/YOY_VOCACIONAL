using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Profesionalizacion.DTO.BO
{
    public class ejetematicoinputdto
    {
        public int? nivelid { get; set; }
        public byte? grado { get; set; }
        public long? ejetematicoid { get; set; }
        public int? areaid { get; set; }
        public int? materiaid { get; set; }
        
        //filtros de búsqueda
        public string nombreeje { get; set; }
        public string nombretema { get; set; }
        public string nombrearea { get; set; }
        public string nombremateria { get; set; }
        public string competencia { get; set; }
        public string aprendizaje { get; set; }

        //paginación
        public int? pagesize { get; set; }
        public int? currentpage { get; set; }
        public string sort { get; set; }
        public string order { get; set; }
    }
}
