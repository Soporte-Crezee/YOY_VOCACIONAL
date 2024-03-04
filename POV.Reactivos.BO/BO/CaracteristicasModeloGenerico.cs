using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Modelo.BO;

namespace POV.Reactivos.BO
{
    /// <summary>
    /// Clase que representa las caracteristicas de reactivos de modelo generico
    /// </summary>
    public class CaracteristicasModeloGenerico : Caracteristicas
    {
        private ModeloDinamico modelo;
        /// <summary>
        /// Modelo relacionado
        /// </summary>
        public ModeloDinamico Modelo
        {
            get { return modelo; }
            set { modelo = value; }
        }
        private Clasificador clasificador;
        /// <summary>
        /// Clasificador relacionado
        /// </summary>
        public Clasificador Clasificador
        {
            get { return clasificador; }
            set { clasificador = value; }
        }
    }
}
