using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.CentroEducativo.BO
{
    public class SesionOrientacion
    {
        public int? SesionOrientacionID { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public string Fecha { get; set; }
        public bool? AsistenciaAspirante { get; set; }
        public bool? AsistenciaOrientador { get; set; }
        public int? CantidadHoras { get; set; }

        public long? AlumnoID { get; set; }
        public virtual Alumno Alumno { get; set; }

        public int? DocenteID { get; set; }
        public virtual Docente Docente { get; set; }

        /// <summary>
        /// Determinar el estado de la sesion
        /// </summary> 
        public ESesionOrientacion? EstatusSesion { get; set; }

        public bool? EncuestaContestada { get; set; }
        public string HoraFinalizado { get; set; }

        public byte[] Version { get; set; }
    }
}
