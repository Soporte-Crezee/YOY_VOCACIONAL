using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Reactivos.BO;
using POV.Prueba.BO;
using POV.CentroEducativo.BO;


namespace POV.Prueba.Diagnostico.Dinamica.BO
{
    public class ResultadoPruebaDinamicaFactory : IResultadoPruebaFactory
    {
        public IResultadoPrueba CreateResultadoPrueba(Alumno alumno, List<Reactivo> reactivos)
        {

            List<ARespuestaReactivo> respuestas = new List<ARespuestaReactivo>();
            foreach (Reactivo reactivo in reactivos)
            {
                List<ARespuestaPregunta> respuestasPregunta = new List<ARespuestaPregunta>();

                foreach (Pregunta pregunta in reactivo.Preguntas)
                {
                    RespuestaPreguntaDinamica respuestaPregunta = new RespuestaPreguntaDinamica();
                    respuestaPregunta.Pregunta = pregunta;
                    respuestaPregunta.EstadoRespuesta = EEstadoRespuesta.NOCONTESTADA;
                    respuestaPregunta.FechaRegistro = DateTime.Now;

                    if (pregunta.RespuestaPlantilla.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.OPCION_MULTIPLE){
                        respuestaPregunta.RespuestaAlumno = new RespuestaDinamicaOpcionMultiple();
                        (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaOpcionMultiple).FechaRegistro = DateTime.Now;
                        (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaOpcionMultiple).ListaOpcionesRespuesta = new List<OpcionRespuestaPlantilla>();
                        (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaOpcionMultiple).Tiempo = 0;
                        (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaOpcionMultiple).TipoRespuestaPlantilla = ETipoRespuestaPlantilla.OPCION_MULTIPLE;

                    } else if (pregunta.RespuestaPlantilla.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.ABIERTA){
                        respuestaPregunta.RespuestaAlumno = new RespuestaDinamicaAbierta();
                        (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaAbierta).FechaRegistro = DateTime.Now;
                        (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaAbierta).RespuestaPlantilla = pregunta.RespuestaPlantilla as RespuestaPlantillaAbierta;
                        (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaAbierta).TipoRespuestaPlantilla = pregunta.RespuestaPlantilla.TipoRespuestaPlantilla;
                    } else if (pregunta.RespuestaPlantilla.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.ABIERTA_NUMERICO){

                    }
                    respuestasPregunta.Add(respuestaPregunta);
                }

                RespuestaReactivoDinamica respuestaReactivo = new RespuestaReactivoDinamica();
                respuestaReactivo.EstadoReactivo = EEstadoReactivo.NOINICIADO;
                respuestaReactivo.FechaRegistro = DateTime.Now;
                respuestaReactivo.Reactivo = reactivo;
                respuestaReactivo.Tiempo = 0;
                respuestaReactivo.ListaRespuestaPreguntas = respuestasPregunta;

                respuestas.Add(respuestaReactivo);

            }



            RegistroPruebaDinamica registroPrueba = new RegistroPruebaDinamica();
            registroPrueba.EstadoPrueba = EEstadoPrueba.NOINICIADA;
            registroPrueba.FechaInicio = DateTime.Now;
            registroPrueba.FechaRegistro = DateTime.Now;
            registroPrueba.ListaRespuestaReactivos = respuestas;
            registroPrueba.Alumno = alumno;


            ResultadoPruebaDinamica resultadoPrueba = new ResultadoPruebaDinamica();

            resultadoPrueba.Alumno = alumno;
            resultadoPrueba.FechaRegistro = DateTime.Now;
            resultadoPrueba.RegistroPrueba = registroPrueba;



            return resultadoPrueba;
        }
    }
}
