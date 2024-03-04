using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Reactivos.BO
{
    /// <summary>
    /// RespuestaPlantillaNumerico
    /// </summary>
    public class RespuestaPlantillaNumerico : RespuestaPlantillaAbierta
    {
        private decimal? margenError;
        /// <summary>
        /// Margen de Error de la RespuestaPlantillaNumerico
        /// </summary>
        public decimal? MargenError
        {
            get { return this.margenError; }
            set { this.margenError = value; }
        }
        private int? numeroDecimales;
        /// <summary>
        /// Numero de Decimales Maximo de la RespuestaPlantillaNumerico
        /// </summary>
        public int? NumeroDecimales
        {
            get { return this.numeroDecimales; }
            set { this.numeroDecimales = value; }
        }
        private ETipoMargen? tipoMargen;
        /// <summary>
        /// 
        /// </summary>
        public ETipoMargen? TipoMargen
        {
            get { return this.tipoMargen; }
            set { this.tipoMargen = value; }
        }
        public string ValidarValorRespuesta()
        {
            throw new NotImplementedException();
        }
    }
}
