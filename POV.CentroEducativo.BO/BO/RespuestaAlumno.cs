using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.CentroEducativo.BO
{
    public class RespuestaAlumno
    {
        public long? AlumnoID { get; set; }
        public int? PruebaID { get; set; }
        public int? ResultadoPruebaID { get; set; }

        /// <summary>
        /// obtiene el nombre de la prueba finalizada
        /// </summary>
        public string Nombre { get; set; }
        
        /// <summary>
        /// obtiene la calificación de la prueba finalizada
        /// </summary>
        public Decimal? Calificacion { get; set; }
        
        /// <summary>
        /// Determinar si la prueba esta finalizada
        /// </summary>
        public byte? EstadoPrueba { get; set; }
        
        /// <summary>
        /// Determinar el tipo de presentacion de la prueba
        /// </summary>
        public byte? TipoPruebaPresentacion { get; set; }
        
        /// <summary>
        /// Determinar si la prueba es Premium ó Gratuita
        /// </summary>
        public EEsPremium EsPremium { get; set; }
    }
}
