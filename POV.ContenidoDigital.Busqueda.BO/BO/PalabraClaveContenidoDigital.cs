using System;
using System.Collections.Generic;
using POV.Profesionalizacion.BO;

namespace POV.ContenidosDigital.Busqueda.BO
{
    // Clase Concreta PalabraClaveContenidoDigital
    /// <summary>
    /// Palabra Clave de Contenido Digital
    /// </summary>
    public class PalabraClaveContenidoDigital : APalabraClaveContenido
    {
        private List<ContenidoDigitalAgrupador> contenidoDigitalAgrupador;
        /// <summary>
        /// ContenidoDigitalAgrupador
        /// </summary>
        public List<ContenidoDigitalAgrupador> ContenidoDigitalAgrupador
        {
            get { return this.contenidoDigitalAgrupador; }
            set { this.contenidoDigitalAgrupador = value; }
        }
    }
}
