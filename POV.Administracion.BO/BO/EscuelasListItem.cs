using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Administracion.BO
{
    public class EscuelasListItem
    {
        public int? IDEscuela { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Turno { get; set; }
        public string LicenciaID { get; set; }
        public string Ambito { get; set; }
        public string Control { get; set; }
        public string Nivel { get; set; }
        public string TipoServicio { get; set; }
        public string Zona { get; set; }
        public string Ubicacion { get; set; }
    }
}
