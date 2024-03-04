using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Modelo.BO;

namespace POV.Reactivos.BO
{
    /// <summary>
    /// Clase que representa una respuesta plantilla generico
    /// </summary>
    public class RespuestaPlantillaGenericoTexto : RespuestaPlantillaTexto
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
        /// <summary>
        /// Clasificador
        /// </summary>
        private Clasificador clasificador;

        public Clasificador Clasificador
        {
            get { return clasificador; }
            set { clasificador = value; }
        }

        public override object Clone()
        {
            RespuestaPlantillaGenericoTexto nuevo = new RespuestaPlantillaGenericoTexto();
            nuevo = (RespuestaPlantillaGenericoTexto)base.Clone();
            nuevo.Clasificador = this.clasificador;
            nuevo.Modelo = this.modelo;

            return nuevo;
        }

    }
}
