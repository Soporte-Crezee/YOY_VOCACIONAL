using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Calificaciones.BO
{
    /// <summary>
    /// DetalleResultadoClasificacion
    /// </summary>
    public class DetalleResultadoClasificacion
    {
        private int? detalleResultadoID;
        /// <summary>
        /// Identificador de DetalleResultadoClasificacion
        /// </summary>
        public int? DetalleResultadoID
        {
            get
            {
                return detalleResultadoID;
            }
            set
            {
                detalleResultadoID = value;
            }
        }
        private decimal? valor;
        /// <summary>
        /// Valor
        /// </summary>
        public decimal? Valor
        {
            get { return this.valor; }
            set { this.valor = value; }
        }
        private bool? esAproximado;
        /// <summary>
        /// EsAproximado
        /// </summary>
        public bool? EsAproximado
        {
            get { return this.esAproximado; }
            set { this.esAproximado = value; }
        }
        private EscalaClasificacionDinamica escalaClasificacionDinamica;
        /// <summary>
        /// EscalaClasificacionDinamica
        /// </summary>
        public EscalaClasificacionDinamica EscalaClasificacionDinamica {
            get { return this.escalaClasificacionDinamica; }
            set { this.escalaClasificacionDinamica = value; }
        }

    }
}
