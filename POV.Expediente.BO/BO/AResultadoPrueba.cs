using System;
using System.Collections.Generic;
using POV.Prueba.BO;

namespace POV.Expediente.BO
{
    /// <summary>
    /// AResultadoPrueba
    /// </summary>
    public abstract class AResultadoPrueba
    {
        private int? resultadoPruebaID;

        public int? ResultadoPruebaID
        {
            get { return this.resultadoPruebaID; }
            set { this.resultadoPruebaID = value; }
        }

        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha en la que se creo el registro de 'ResultadoPruebaDiagnostico'
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }

        private APrueba prueba;
        /// <summary>
        /// Prueba asignada
        /// </summary>
        public APrueba Prueba
        {
            get { return this.prueba; }
            set { this.prueba = value; }
        }

        #region Ajuste 2
        private ETipoResultadoPrueba? tipo;

        public ETipoResultadoPrueba? Tipo
        {
            get { return this.tipo; }
            set { this.tipo = value; }
        }
        #endregion

        public abstract string Nombre();
        public abstract List <string> Resultados();
    }
}
