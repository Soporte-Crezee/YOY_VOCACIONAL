using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Reactivos.BO;

namespace POV.Prueba.BO
{
    /// <summary>
    /// Clase que representa una registro de respuesta abierta
    /// </summary>
    public abstract class ARespuestaAbierta : ARespuestaAlumno
    {
        private RespuestaPlantillaAbierta respuestaPlantillaAbierta;

        /// <summary>
        /// RespuestaplantillaAbierta
        /// </summary>
        public RespuestaPlantillaAbierta RespuestaPlantilla
        {
            get { return this.respuestaPlantillaAbierta; }
            set { this.respuestaPlantillaAbierta = value; }
        }

        private string textoRespuesta;

        public string TextoRespuesta
        {
            get { return this.textoRespuesta; }
            set { this.textoRespuesta = value; }
        }
        private decimal? valorRespuesta;

        public decimal? ValorRespuesta
        {
            get { return this.valorRespuesta; }
            set { this.valorRespuesta = value; }
        }
    }
}
