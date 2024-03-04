using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Prueba.BO
{
    public class PruebaFilter : APrueba
    {
        public override ETipoPrueba TipoPrueba
        {
            get { throw new NotImplementedException(); }
        }

        private int? modeloID;
        /// <summary>
        /// Identificador del modelo de pruebas
        /// </summary>
        public int? ModeloID
        {
            get { return this.modeloID; }
            set { this.modeloID = value; }
        }
    }
}
