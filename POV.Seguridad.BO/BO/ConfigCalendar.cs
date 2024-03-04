using POV.Seguridad.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Seguridad.BO 
{
    public class ConfigCalendar
    {
        public long? ConfigCalendarID { get; set; }
        public int? UsuarioID { get; set; }
        public string DiasLaborales { get; set; }
        public string InicioTrabajo { get; set; }
        public string FinTrabajo { get; set; }
        public string InicioDescanso { get; set; }
        public string FinDescanso { get; set; }

        public virtual Usuario Usuario{ get; set; }

        public virtual List<EventCalendar> EventCalendar { get; set; }
    }
}
