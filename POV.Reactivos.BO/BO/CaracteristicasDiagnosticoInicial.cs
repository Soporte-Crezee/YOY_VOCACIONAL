using System;
using System.Collections.Generic;
using System.Linq;

namespace POV.Reactivos.BO
{
    public class CaracteristicasDiagnosticoInicial: Caracteristicas
    {
        private Byte? edad;
        /// <summary>
        /// Característica de Edad en el Diagnostico Inicial
        /// </summary>
        public Byte? Edad 
        {
            get { return this.edad; }
            set { this.edad = value; }
        }
    }
}
