using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Reactivos.Validaciones.BO
{
    /// <summary>
    /// Clase encargada de controlar la respuesta de la validacion
    /// </summary>
    public class RespuestaValidacionReactivo
    {
        private bool esValido;
        /// <summary>
        /// Indica si es valida la respuesta
        /// </summary>
        public bool EsValido
        {
            get { return esValido; }
            set { esValido = value; }
        }

        private string error;
        /// <summary>
        /// Cadena que representa el error de validacion
        /// </summary>
        public string Error
        {
            get { return error; }
            set { error = value; }
        }
    }
}
