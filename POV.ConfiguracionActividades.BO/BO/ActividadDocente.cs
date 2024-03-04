using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.CentroEducativo.BO;
using POV.Seguridad.BO;

namespace POV.ConfiguracionActividades.BO
{
    public class ActividadDocente : Actividad
    {
        public virtual Docente Docente { get; set; }
        public int? DocenteId { get; set; }

        public virtual Usuario Usuario { get; set; }

        public int? UsuarioId { get; set; }

    }
}
