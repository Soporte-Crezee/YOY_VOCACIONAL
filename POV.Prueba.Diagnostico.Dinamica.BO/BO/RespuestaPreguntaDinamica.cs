using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Prueba.BO;
using POV.Reactivos.BO;

namespace POV.Prueba.Diagnostico.Dinamica.BO
{
    /// <summary>
    /// Representa un registro de respuesta de pregunta
    /// </summary>
    public class RespuestaPreguntaDinamica : ARespuestaPregunta
    {
        public override object Clone()
        {
            RespuestaPreguntaDinamica nuevo = (RespuestaPreguntaDinamica)base.Clone();
            return nuevo;
        }

        /// <summary>
        /// Calcula el Valor de una pregunta
        /// </summary>
        /// <returns>Valor de una Pregunta</returns>
        public decimal CalcularPuntajePregunta()
        {
            decimal puntaje = 0;
            if (this.RespuestaAlumno is RespuestaDinamicaOpcionMultiple)
            {
                List<OpcionRespuestaPlantilla> listaRespuestasAlumno = ((RespuestaDinamicaOpcionMultiple)this.RespuestaAlumno).ListaOpcionesRespuesta;
                List<OpcionRespuestaPlantilla> listaRespuestasCorrectas = (this.Pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ObtenerOpcionesCorrectas();

                RespuestaDinamicaOpcionMultiple respuestaDinamicaOpcionMultiple = this.RespuestaAlumno as RespuestaDinamicaOpcionMultiple;
                RespuestaPlantillaOpcionMultiple respuestaPlantillaOpcionMultiple = this.Pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple;

                if (respuestaDinamicaOpcionMultiple != null && respuestaPlantillaOpcionMultiple != null)
                {
                    if (respuestaPlantillaOpcionMultiple.ModoSeleccion != null)
                    {
                        if (respuestaPlantillaOpcionMultiple.ModoSeleccion == EModoSeleccion.UNICA)
                        {
                            bool esCorrrecta = false;
                            if (listaRespuestasAlumno.Count == 1)
                            {
                                foreach (OpcionRespuestaPlantilla opcion in listaRespuestasAlumno)
                                {
                                    esCorrrecta = listaRespuestasCorrectas.FirstOrDefault(item => item.OpcionRespuestaPlantillaID == opcion.OpcionRespuestaPlantillaID) != null;
                                    if (esCorrrecta == true)
                                    {
                                        puntaje = this.Pregunta.Valor.Value;
                                    }
                                    else
                                    {
                                        puntaje = 0;
                                    }
                                }

                            }
                            else
                            {
                                puntaje = 0;
                            }
                        }
                        if (respuestaPlantillaOpcionMultiple.ModoSeleccion == EModoSeleccion.RANGO)
                        {
                            puntaje = 0;
                        }
                        if (respuestaPlantillaOpcionMultiple.ModoSeleccion == EModoSeleccion.MULTIPE)
                        {
                            bool existeEnCorrectas = false;
                            int respuestasAlumno = listaRespuestasAlumno.Count;

                            if (respuestasAlumno != listaRespuestasCorrectas.Count)
                            {
                                puntaje = 0;
                            }
                            else
                            {
                                foreach (OpcionRespuestaPlantilla opcionRespuesta in listaRespuestasAlumno)
                                {
                                    puntaje = this.Pregunta.Valor.Value;
                                    existeEnCorrectas = listaRespuestasCorrectas.FirstOrDefault(p => p.OpcionRespuestaPlantillaID == opcionRespuesta.OpcionRespuestaPlantillaID) != null;
                                    if (existeEnCorrectas == false)
                                    {
                                        puntaje = 0;
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }
            }
            return puntaje;
        }
        /// <summary>
        /// Calcula el puntaje de una pregunta con opciones siempre correctas
        /// </summary>
        /// <returns>Puntaje de la Pregunta</returns>
        public decimal CalcularPuntajeMaximoPregunta()
        {
            decimal puntajeMaximo = 0;

            if (this.RespuestaAlumno is RespuestaDinamicaOpcionMultiple)
            {
                if (this.Pregunta != null && this.Pregunta.RespuestaPlantilla != null)
                {
                    if (this.Pregunta.RespuestaPlantilla.TipoPuntaje != null)
                    {
                        if (this.Pregunta.RespuestaPlantilla.TipoPuntaje == ETipoPuntaje.UNICO)
                        {
                            puntajeMaximo = this.Pregunta.Valor.Value;
                        }
                        if (this.Pregunta.RespuestaPlantilla.TipoPuntaje == ETipoPuntaje.SINPUNTAJE)
                        {
                            puntajeMaximo = 0;
                        }
                        if (this.Pregunta.RespuestaPlantilla.TipoPuntaje == ETipoPuntaje.POROPCION)
                        {
                            puntajeMaximo = this.Pregunta.Valor.Value;
                        }
                    }
                }
            }

            return puntajeMaximo;
        }
        /// <summary>
        /// Obtiene el valor de la respuesta del alumno a la pregunta
        /// </summary>
        /// <returns>decimal: determina el valor de la respuesta contestada.</returns>
        public decimal CalificarRespuestaPreguntaDinamica()
        {
            if (RespuestaAlumno is RespuestaDinamicaOpcionMultiple)
            {
                List<OpcionRespuestaPlantilla> listaRespuestasAlumno = (RespuestaAlumno as RespuestaDinamicaOpcionMultiple).ListaOpcionesRespuesta;
                List<OpcionRespuestaPlantilla> listaRespuestasCorrectas = (Pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ObtenerOpcionesCorrectas();
                if ((Pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ModoSeleccion == EModoSeleccion.UNICA)
                {
                    if (ValidarRespuestaCorrectaUnica(listaRespuestasAlumno, listaRespuestasCorrectas))
                        return 1;
                }
                if ((Pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ModoSeleccion == EModoSeleccion.MULTIPE)
                {
                    if (ValidarRespuestaCorrectaMultiple(listaRespuestasAlumno, listaRespuestasCorrectas))
                        return 1;
                }
            }
            return 0;
        }
        /// <summary>
        /// Valida que la respuesta del alumno es correcta
        /// </summary>
        /// <returns>Bool: determina si la respuesta es correcta.</returns>
        private bool ValidarRespuestaCorrectaUnica(List<OpcionRespuestaPlantilla> listaRespuestasAlumno, List<OpcionRespuestaPlantilla> listaRespuestasCorrectas)
        {
            //Validamos que el alumno selecciono solo una respuesta a la pregunta
            if (listaRespuestasAlumno.Count == 1)
            {
                //List<OpcionRespuestaPlantilla> respuestaCorrecta = listaRespuestasCorrectas.Intersect(listaRespuestasAlumno).ToList();
                var listaRespuestaCorrecta = from respuestaAlumno in listaRespuestasAlumno
                                             from respuestaCorrecta in listaRespuestasCorrectas
                                             where respuestaAlumno.OpcionRespuestaPlantillaID == respuestaCorrecta.OpcionRespuestaPlantillaID
                                             select respuestaAlumno;
                if (listaRespuestaCorrecta.Count() > 0)
                    return true;
            }
            return false;

        }
        /// <summary>
        /// Valida que las respuestas del alumno sean correctas
        /// </summary>
        /// <returns>Bool: determina si las respuestas son correctas.</returns>
        public bool ValidarRespuestaCorrectaMultiple(List<OpcionRespuestaPlantilla> listaRespuestasAlumno, List<OpcionRespuestaPlantilla> listaRespuestasCorrectas)
        {
            int numeroListaAlumno = listaRespuestasAlumno.Count;
            int numeroListaRespuestas = listaRespuestasCorrectas.Count;
            //Validamos que las respuestas del alumno sean iguales  als respuestas de la pregunta
            if (numeroListaAlumno != numeroListaRespuestas)
                return false;
            //Validamos que el numero de respuestas del alumno sean iguales al numero de respuestas de la pregunta
            else
            {
                //List<OpcionRespuestaPlantilla> respuestaCorrecta = listaRespuestasCorrectas.Intersect(listaRespuestasAlumno).ToList();
                var listaRespuestaCorrecta = from respuestaAlumno in listaRespuestasAlumno
                                             from respuestaCorrecta in listaRespuestasCorrectas
                                             where respuestaAlumno.OpcionRespuestaPlantillaID == respuestaCorrecta.OpcionRespuestaPlantillaID
                                             select respuestaAlumno;
                List<OpcionRespuestaPlantilla> listaCorrecta = listaRespuestaCorrecta.ToList();
                if (listaRespuestaCorrecta.Count() == numeroListaRespuestas)
                    return true;
            }
            return false;
        }
    }
}
