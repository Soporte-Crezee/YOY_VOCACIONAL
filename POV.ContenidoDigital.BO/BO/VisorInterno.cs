using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.ContenidosDigital.BO
{
    /// <summary>
    /// Clase que representa un visor interno
    /// </summary>
    public class VisorInterno : AVisor
    {
        private string extension;

        public string Extension
        {
            get { return extension; }
            set { extension = value; }
        }


        public override bool? EsInterno
        {
            get { return true; }
        }
    }
}
