using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Reactivos.BO;

namespace POV.Reactivos.Validaciones.BO
{
    /// <summary>
    /// Interface que representa un algoritmo de validación de los reactivos
    /// </summary>
    public interface IValidadorReactivo
    {
        /// <summary>
        /// Valida la información de un reactivo
        /// </summary>
        /// <param name="reactivo">Reactivo que se desea validar</param>
        /// <returns>Respuesta de la validación</returns>
        RespuestaValidacionReactivo Validar(Reactivo reactivo);
    }
}
