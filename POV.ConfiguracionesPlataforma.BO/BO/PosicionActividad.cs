using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.ConfiguracionesPlataforma.BO
{
    public class PosicionActividad
    {
        //Identificador de la Posicion de la Actividad
        public int? PosicionActividadId { get; set; }

        //Posicion X de la Actividad
        public decimal? PosicionX { get; set; }

        //Posicion Y de la Actividad
        public decimal? PosicionY { get; set; }

        //Orden de la Actividad
        public int? Orden { get; set; }

        //Versionado
        public byte[] Version { get; set; }
    }
}
