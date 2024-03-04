using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.ContenidosDigital.BO
{
    /// <summary>
    /// Clase que representa un visor externo
    /// </summary>
    public class VisorExterno : AVisor
    {
        private string fuente;

        public string Fuente
        {
            get { return fuente; }
            set { fuente = value; }
        }



        public override bool? EsInterno
        {
            get { return false; }
        }
    }
}
