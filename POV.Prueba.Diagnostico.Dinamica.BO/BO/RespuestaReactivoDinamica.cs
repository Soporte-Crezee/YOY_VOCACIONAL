using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Prueba.BO;
using POV.Reactivos.BO;
using POV.Modelo.BO;


namespace POV.Prueba.Diagnostico.Dinamica.BO
{
    /// <summary>
    /// Representa una respuesta de reactivo
    /// </summary>
    public class RespuestaReactivoDinamica : ARespuestaReactivo
    {
        /// <summary>
        /// Calcula los resultados de las preguntas de una prueba por seleccion de clasificadores
        /// </summary>
        /// <returns>null si la prueba no está bien configurada, throws a exception si la prueba,las preguntas, o el reactivo, no son del tipo esperado</returns>
        public   IDictionary<int,decimal> CalcularResultadoMetodoSeleccion() {
            IDictionary<int, decimal> parcial = new Dictionary<int, decimal>();

             ARespuestaOpcionMultiple respuestaOpcionMultiple = null;
             foreach (ARespuestaPregunta respuestaPregunta in this.ListaRespuestaPreguntas)
             {
                   
                 RespuestaPreguntaDinamica respuestaPreguntaDinamica = respuestaPregunta as RespuestaPreguntaDinamica;
                 if (respuestaPreguntaDinamica == null)
                     throw new Exception("La respuesta de la pregunta no es de una prueba dinámica");
                 ARespuestaAlumno respuestaAlumno = respuestaPreguntaDinamica.RespuestaAlumno;

                 if (respuestaAlumno == null)
                     throw new Exception("Respuesta no contestada");

                 if (respuestaAlumno.TipoRespuestaPlantilla != ETipoRespuestaPlantilla.OPCION_MULTIPLE)
                     throw new Exception("La respuesta no es de tipo opción múltiple");     
                 
                     respuestaOpcionMultiple = respuestaAlumno as ARespuestaOpcionMultiple;
                     if (respuestaAlumno == null)
                         throw new Exception("La respuesta no es de tipo opción múltiple");                        
                 
                 foreach (OpcionRespuestaPlantilla opcion in respuestaOpcionMultiple.ListaOpcionesRespuesta)
                 {
                     OpcionRespuestaModeloGenerico opcionSeleccionada=null;
                     if(opcion is OpcionRespuestaModeloGenerico)
                        opcionSeleccionada = (OpcionRespuestaModeloGenerico)opcion;
                     else throw new Exception("Opcion respuesta plantilla no es de un modelo genérico");

                     if (opcionSeleccionada.Clasificador == null)
                         return null;
                     if (opcionSeleccionada.Clasificador.ClasificadorID == null)
                         return null;
                     
                     int index = EsRegistroRepetido(parcial, opcionSeleccionada.Clasificador.ClasificadorID.Value);
                    
                     if (index >= 0)     parcial[index]++;
                     else   parcial.Add(opcionSeleccionada.Clasificador.ClasificadorID.Value, 1);
                     
                 }
               }                      
            return parcial;        
          }
            
        private int EsRegistroRepetido(IDictionary <int,decimal> result, int clasificadorID){

            if (result.ContainsKey(clasificadorID))
                return clasificadorID;
            return -1;
           
        }


        public override object Clone()
        {
            RespuestaReactivoDinamica respuestaReactivo = (RespuestaReactivoDinamica)base.Clone();

            if (this.ListaRespuestaPreguntas != null)
            {
                respuestaReactivo.ListaRespuestaPreguntas = new List<ARespuestaPregunta>();
                foreach (RespuestaPreguntaDinamica respuestaPregunta in ListaRespuestaPreguntas)
                {
                    respuestaReactivo.ListaRespuestaPreguntas.Add(respuestaPregunta.Clone() as RespuestaPreguntaDinamica);
                }

            }
            return respuestaReactivo;
        }

        /// <summary>
        /// Calcula el puntaje de un Reactivo
        /// </summary>
        /// <returns>Puntaje final de un reactivo</returns>
        public decimal CalcularPuntajeReactivo()
        {
            decimal puntajeFinal = 0;

            foreach (RespuestaPreguntaDinamica respuestaPreguntaDinamica in this.ListaRespuestaPreguntas)
            {
                puntajeFinal += respuestaPreguntaDinamica.CalcularPuntajePregunta();
            }

            return puntajeFinal;
        }

        /// <summary>
        /// Calcula el puntaje de un reactivo con respuestas siempre correctas
        /// </summary>
        /// <returns>Puntaje maximo de un reactivo</returns>
        public decimal CalcularPuntajeMaximoReactivo()
        {
            decimal puntajeMaximo = 0;

            foreach (RespuestaPreguntaDinamica respuesta in this.ListaRespuestaPreguntas)
            {
                puntajeMaximo += respuesta.CalcularPuntajeMaximoPregunta();
            }

            return puntajeMaximo;
        }
        /// <summary>
        /// Calcula el resultado de una prueba por método de Clasificación.
        /// </summary>
        /// <returns></returns>
        public IDictionary<int, decimal> CalcularResultadoMetodoClasificacion()
        {
            IDictionary<int, decimal> parte = new Dictionary<int, decimal>();

            ARespuestaOpcionMultiple respuestaOpcionMultiple = null;
            foreach (ARespuestaPregunta respuestaPregunta in this.ListaRespuestaPreguntas)
            {
                RespuestaPreguntaDinamica respuestaPreguntaDinamica = respuestaPregunta as RespuestaPreguntaDinamica;
                if (respuestaPreguntaDinamica == null)
                    throw new Exception("La respuesta de la pregunta no es de una prueba dinámica");
                ARespuestaAlumno respuestaAlumno = respuestaPreguntaDinamica.RespuestaAlumno;

                if (respuestaAlumno == null)
                    throw new Exception("Respuesta no contestada");

                if (respuestaAlumno.TipoRespuestaPlantilla != ETipoRespuestaPlantilla.OPCION_MULTIPLE)
                    throw new Exception("La respuesta no es de tipo opción múltiple");

                respuestaOpcionMultiple = respuestaAlumno as ARespuestaOpcionMultiple;
                if (respuestaAlumno == null)
                    throw new Exception("La respuesta no es de tipo opción múltiple");

                foreach (OpcionRespuestaPlantilla opcion in respuestaOpcionMultiple.ListaOpcionesRespuesta)
                {
                    OpcionRespuestaModeloGenerico opcionSeleccionada = null;
                    if (opcion is OpcionRespuestaModeloGenerico)
                        opcionSeleccionada = (OpcionRespuestaModeloGenerico)opcion;
                    else throw new Exception("Opcion respuesta plantilla no es de un modelo genérico");

                    if (opcionSeleccionada.Clasificador == null)
                        return null;
                    if (opcionSeleccionada.Clasificador.ClasificadorID == null)
                        return null;

                    int index = EsRegistroRepetido(parte, opcionSeleccionada.Clasificador.ClasificadorID.Value);

                    if (index >= 0)
                    {
                        parte[index] += opcionSeleccionada.PorcentajeCalificacion.Value;
                    }
                    else
                    {
                        parte.Add(opcionSeleccionada.Clasificador.ClasificadorID.Value, opcionSeleccionada.PorcentajeCalificacion.Value);
                    }
                }
            }
            return parte;
        }

      }
        
          
}
