using GP.SocialEngine.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Seguridad.BO
{
    public class EventCalendar
    {
        public long? EventCalendarID { get; set; }
        public int? UsuarioID { get; set; }
        public string Asunto { get; set; }
        public string Fecha { get; set; }
        public string HrsInicio { get; set; }
        public string HrsFin { get; set; }
        public virtual Usuario Usuario { get; set; }

        // alumno
        public long? AlumnoID { get; set; }
        public string NombreCompletoAlumno { get; set; }

        public int? CantidadHoras { get; set; }

        public long? ConfigCalendarID { get; set; }
        public virtual ConfigCalendar ConfigCalendar { get; set; }
    }
}
