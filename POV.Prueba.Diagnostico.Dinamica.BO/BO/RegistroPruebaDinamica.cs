using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Prueba.BO;
using POV.Modelo.BO;

namespace POV.Prueba.Diagnostico.Dinamica.BO
{
    /// <summary>
    /// Representa un registro de prueba dinamica
    /// </summary>
    public class RegistroPruebaDinamica : ARegistroPrueba
    {
        /// <summary>
        /// Obtiene el resultado de seleccion de clasificadorse de una lista de reactivos contenidos en la prueba
        /// </summary>
        /// <returns>null si la prueba no está bien configurada, throws a exception si la prueba,las preguntas, o el reactivo, no son del tipo esperado</returns>
        public IDictionary<int, decimal> ResultadoMetodoCalificacionSeleccionClasificadores()
        {
            IDictionary<int, decimal> results = new Dictionary<int, decimal>();            
            IDictionary<int, decimal> simpleAnswers = null;

            foreach (ARespuestaReactivo _respuestaReactivo in this.ListaRespuestaReactivos)
            {
               
                if (_respuestaReactivo is RespuestaReactivoDinamica)
                {
                    try
                    {
                         simpleAnswers = (_respuestaReactivo as RespuestaReactivoDinamica).CalcularResultadoMetodoSeleccion();
                    }
                    catch (Exception e) { throw e; }
                    if (simpleAnswers == null)
                        return null;
                    foreach (KeyValuePair<int, decimal> parValoresRespuesta in simpleAnswers)
                    {
                        if (results.ContainsKey(parValoresRespuesta.Key))
                            results[parValoresRespuesta.Key]++;
                        else results.Add(parValoresRespuesta.Key, parValoresRespuesta.Value);

                    }
                }
                else throw new Exception("La respuesta del reactivo no es del tipo esperado");

            }
            return results;
        }

        /// <summary>
        /// Calcula el Puntaje Final de la prueba
        /// </summary>
        /// <returns>puntajeFinalPrueba</returns>
        public decimal CalcularPuntajePrueba()
        {
            decimal puntajeFinalPrueba = 0;

            foreach (RespuestaReactivoDinamica respuestaReactivoDinamica in this.ListaRespuestaReactivos)
            {
                puntajeFinalPrueba += respuestaReactivoDinamica.CalcularPuntajeReactivo();
            }

            return puntajeFinalPrueba;
        }

        /// <summary>
        /// Calcula el puntaje maxima de una prueba con todas los reactivos corrrectos
        /// </summary>
        /// <returns>puntaje maxima prueba</returns>
        public decimal CalcularPuntajePosiblePrueba()
        {
            decimal puntaje = 0;

            foreach (RespuestaReactivoDinamica respuesta in this.ListaRespuestaReactivos)
            {
                puntaje += respuesta.CalcularPuntajeMaximoReactivo();
            }

            return puntaje;
        }
        /// <summary>
        /// Obtiene el resultado de clasificacion de una lista de Reactivos de la prueba. 
        /// </summary>
        /// <returns></returns>
        public IDictionary<int, decimal> ResultadoMetodoCalificacionClasificacion()
        {
            IDictionary<int, decimal> result = new Dictionary<int, decimal>();
            IDictionary<int, decimal> respuesta = null;

            foreach (ARespuestaReactivo respuestaReactivo in this.ListaRespuestaReactivos)
            {
                if (respuestaReactivo is RespuestaReactivoDinamica)
                {
                    try
                    {
                        respuesta =
                            (respuestaReactivo as RespuestaReactivoDinamica).CalcularResultadoMetodoClasificacion();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    if (respuesta == null)
                        return null;
                    foreach (KeyValuePair<int, decimal> valoresRespuesta in respuesta)
                    {
                        if (result.ContainsKey(valoresRespuesta.Key))
                        {
                            result[valoresRespuesta.Key] += valoresRespuesta.Value;
                        }
                        else
                        {
                            result.Add(valoresRespuesta.Key, valoresRespuesta.Value);
                        }
                    }
                }
                else throw new Exception("Se espera otro tipo de respuesta del reactivo.");
            }
            return result;
        }
    }

}