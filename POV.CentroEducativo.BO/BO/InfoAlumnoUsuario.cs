using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Modelo.BO;
using POV.Comun.BO;
using POV.Localizacion.BO;

namespace POV.CentroEducativo.BO
{
    public class InfoAlumnoUsuario
    {
        public long? AlumnoID { get; set; }
        public int? UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public Clasificador clasificador { get; set; }
        public string Escuela { get; set; }
        public EGrado? Grado { get; set; }
        public Estado estado { get; set; }
        public string Email { get; set; }
        public bool? Premium { get; set; }
        public bool? RecibirInformacion { get; set; }
        public bool? DatosCompletos { get; set; }
    }
}
