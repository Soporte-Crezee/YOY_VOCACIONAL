using POV.Localizacion.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.CentroEducativo.BO
{
    public class Universidad: ICloneable
    {
        public long? UniversidadID {get; set;}
        public string NombreUniversidad {get; set;}
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public Ubicacion Ubicacion { get; set; }
        public long UbicacionID { get; set; }
        public string PaginaWEB { get; set; }
        public string CoordinadorCarrera { get; set; }
        public bool? Activo { get; set; }
        public List<Carrera> Carreras { get; set; }
        public string ClaveEscolar { get; set; }
        public List<Docente> Docentes { get; set; }
        public virtual List<EventoUniversidad> EventosUniversidad { get; set; }
        public List<Alumno> Alumnos { get; set; }
        public string Siglas { get; set; }
        public ENivelEscolar? NivelEscolar { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
