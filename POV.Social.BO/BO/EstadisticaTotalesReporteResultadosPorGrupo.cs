using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Social.BO
{
    /// <summary>
    /// Datos estadísticos de los alumnos que presentaron la prueba diagnóstica.
    /// </summary>
    public class EstadisticaTotalesReporteResultadosPorGrupo
    {
        private string datoEstadistico;
        /// <sumary>
        /// Tipo de dato almacenado.
        /// </sumary>
        public string DatoEstadistico
        {
            get { return this.datoEstadistico; }
            set { this.datoEstadistico = value; }
        }

        private int cifraEstadistica;
        /// <sumary>
        /// Cifra numérica correspondiente al dato estadístico.
        /// </sumary>
        public int CifraEstadistica
        {
            get { return this.cifraEstadistica;  }
            set { this.cifraEstadistica = value; }
        }
    }
}
