using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Expediente.BO;
using POV.Modelo.BO;

namespace POV.Prueba.Diagnostico.Dinamica.BO
{
    /// <summary>
    /// Resultado del clasificador prueba dinamica
    /// </summary>
    public class ResultadoClasificadorDinamica : AResultadoClasificador
    {
        private Clasificador clasificador;
        /// <summary>
        /// Clasificador
        /// </summary>
        public Clasificador Clasificador 
        {
            get { return this.clasificador; }
            set { this.clasificador = value; }
        }

        public override ETipoResultadoClasificador TipoResultadoClasificador
        {
            get { return ETipoResultadoClasificador.DINAMICA; }
        }
    }
}
