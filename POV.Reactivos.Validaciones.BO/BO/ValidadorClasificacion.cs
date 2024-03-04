using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Reactivos.BO;
using POV.Modelo.BO;

namespace POV.Reactivos.Validaciones.BO
{
    /// <summary>
    /// Clase especifica de la validacion del metodo de calificiacion por clasificacion
    /// </summary>
    public class ValidadorClasificacion : IValidadorReactivo
    {
        /// <summary>
        /// Valida la información de un reactivo
        /// </summary>
        /// <param name="reactivo">Reactivo que se desea validar</param>
        /// <returns>Respuesta de la validación</returns>
        public RespuestaValidacionReactivo Validar(Reactivo reactivo)
        {
            if (reactivo == null) throw new ArgumentNullException("ValidadorPuntos: reactivo no puede ser nulo");
            if (reactivo.Caracteristicas == null) throw new ArgumentNullException("ValidadorPuntos:caracteristicas no puede ser nulo");
            if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo == null) throw new ArgumentNullException("ValidadorPuntos: el modelo no puede ser nulo");
            if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo.MetodoCalificacion == null) throw new ArgumentNullException("ValidadorPuntos: el método de calificación no puede ser nulo");

            if (reactivo.Preguntas == null) throw new ArgumentNullException("ValidadorPuntos: la lista de preguntas no puede ser nula");
            if (reactivo.Preguntas.Count == 0) throw new ArgumentException("ValidadorPuntos: la lista de preguntas no puede ser vacia");
            foreach (Pregunta pregunta in reactivo.Preguntas)
            {
                if (pregunta.RespuestaPlantilla is RespuestaPlantillaOpcionMultiple)
                    if ((pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.Count == 0)
                        throw new ArgumentException("ValidadorPuntos: la lista de opciones no puede ser vacia");


            }

            if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo.MetodoCalificacion != EMetodoCalificacion.CLASIFICACION)
                throw new ArgumentException("ValidadorPuntos: el método de calificación debe ser por clasificacion");

            RespuestaValidacionReactivo respuesta = new RespuestaValidacionReactivo();
            respuesta.Error = string.Empty;
            respuesta.EsValido = true;

            ModeloDinamico modelo = (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo;
            foreach (Pregunta pregunta in reactivo.Preguntas)
            {
                if (pregunta.RespuestaPlantilla is RespuestaPlantillaOpcionMultiple)
                {
                    bool existeSinClasificador = (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.Any(item => (item as OpcionRespuestaModeloGenerico).Clasificador == null || (item as OpcionRespuestaModeloGenerico).Clasificador.ClasificadorID == null);
                    if (existeSinClasificador)
                    {
                        respuesta.EsValido = false;
                        respuesta.Error += ", las opciones deben tener clasificador asociado";
                    }

                     bool existeSinPuntaje = (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.Any(item => item.PorcentajeCalificacion == null);
                        if (existeSinPuntaje)
                        {
                            respuesta.EsValido = false;
                            respuesta.Error += ", se debe asignar un puntaje a las opciones de la pregunta";
                        }
                    
                }
            }



            return respuesta;
        }
    }
}
