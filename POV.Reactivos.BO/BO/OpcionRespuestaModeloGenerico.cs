using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Modelo.BO;

namespace POV.Reactivos.BO
{
    /// <summary>
    /// Clase que representa la opcion de un reactivo de modelo generico
    /// </summary>
    public class OpcionRespuestaModeloGenerico : OpcionRespuestaPlantilla
    {
        private ModeloDinamico modelo;
        /// <summary>
        /// Modelo
        /// </summary>
        public ModeloDinamico Modelo
        {
            get { return modelo; }
            set { modelo = value; }
        }

        private Clasificador clasificador;
        /// <summary>
        /// Clasificador
        /// </summary>
        public Clasificador Clasificador
        {
            get { return clasificador; }
            set { clasificador = value; }
        }
    }
}
