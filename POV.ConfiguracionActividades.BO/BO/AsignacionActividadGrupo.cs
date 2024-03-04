using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POV.CentroEducativo.BO;

namespace POV.ConfiguracionActividades.BO
{
    public class AsignacionActividadGrupo
    {
        public long? AsignacionActividadGrupoID { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public virtual GrupoCicloEscolar GrupoCicloEscolar { get; set; }

        public Guid? GrupoCicloEscolarID { get; set; }

        public virtual ActividadDocente ActividadDocente { get; set; }

        public long? ActividadID { get; set; }

        public virtual List<AsignacionActividad> AsignacionesActividades { get; set; }
    }
}
