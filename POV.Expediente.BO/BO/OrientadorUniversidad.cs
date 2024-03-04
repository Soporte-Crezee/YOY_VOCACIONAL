using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Expediente.BO
{
    public class OrientadorUniversidad
    {
        public int? UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public int? DocenteID { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public long? UniversidadID { get; set; }
        public string NombreUniversidad { get; set; }

        public string NombreCompletoOrientador
        {
            get { return (this.Nombre + " " + PrimerApellido + " " + SegundoApellido).Trim(); }
        }
    }
}
