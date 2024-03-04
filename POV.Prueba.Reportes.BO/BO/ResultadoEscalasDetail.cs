using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Prueba.Reportes.BO
{
    /// <summary>
    /// Representa un detalle de escalas del clasificador.
    /// </summary>
    public class ResultadoEscalasDetail
    {
        private int? puntajeID;
        /// <summary>
        /// El identificador del puntaje.
        /// </summary>
        public int? PuntajeID { get { return this.puntajeID; } set { this.puntajeID = value; } }

        private decimal puntajeMinimo;
        /// <summary>
        /// Puntaje minimo de la escala.
        /// </summary>
        public decimal PuntajeMinimo { get { return this.puntajeMinimo; } set { this.puntajeMinimo = value; } }

        private decimal puntajeMaximo;
        /// <summary>
        /// Puntaje máximo de la escala.
        /// </summary>
        public decimal PuntajeMaximo { get { return puntajeMaximo; } set { this.puntajeMaximo = value; } }

        private int? clasificadorID;
        /// <summary>
        /// Clasificador al que pertenece la escala.
        /// </summary>
        public int? ClasificadorID { get { return clasificadorID; } set { this.clasificadorID = value; } }

        private string nombre;
        /// <summary>
        /// Nombre de la escala.
        /// </summary>
        public string Nombre { get { return nombre; } set { this.nombre = value; } }

        private string descripcion;
        /// <summary>
        /// Descripción de la escala.
        /// </summary>
        public string Descripcion
        { get { return this.descripcion; } set { this.descripcion = value; } }
    }
}
