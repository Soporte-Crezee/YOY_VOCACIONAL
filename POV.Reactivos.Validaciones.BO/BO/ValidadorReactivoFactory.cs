using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Reactivos.BO;
using POV.Modelo.BO;

namespace POV.Reactivos.Validaciones.BO
{
    /// <summary>
    /// Clase que representa una fabrica para los diferentes validadores de reactivos
    /// </summary>
    public class ValidadorReactivoFactory
    {
        /// <summary>
        /// Crea un Validador concreto de acuerdo al modelo asociado del reacivo
        /// </summary>
        /// <param name="reactivo">Reactivo</param>
        /// <returns>Validador concreto</returns>
        public IValidadorReactivo Create(Reactivo reactivo){
            IValidadorReactivo validador = null;

            switch ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo.MetodoCalificacion)
            {
                case EMetodoCalificacion.CLASIFICACION:
                    validador = new ValidadorClasificacion();
                    break;
                case EMetodoCalificacion.SELECCION:
                    validador = new ValidadorSeleccion();
                    break;
                case EMetodoCalificacion.PUNTOS:
                    validador = new ValidadorPuntos();
                    break;
                case EMetodoCalificacion.PORCENTAJE:
                    validador = new ValidadorPorcentaje();
                    break;
                default:
                    throw new Exception("Método de calificación no soportado");
            }

            return validador;
        }
    }
}
