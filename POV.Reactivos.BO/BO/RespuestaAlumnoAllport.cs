using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Reactivos.BO
{
    public class RespuestaAlumnoAllport
    {
        public string Clave { get; set; }
        public int? ClasificadorID { get; set; }
        public string Opcion { get; set; }
        public string Respuesta { get; set; }
        public int? Valor { get; set; }
        public int? Grupo { get; set; }        
    }
}
