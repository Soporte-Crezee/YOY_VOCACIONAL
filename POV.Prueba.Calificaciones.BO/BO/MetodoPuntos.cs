using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Modelo.BO;

namespace POV.Prueba.Calificaciones.BO
{
    public class MetodoPuntos : IPoliticaCalificacion
    {
        /// <summary>
        /// Calcula el resultado de una prueba dinamica
        /// </summary>
        /// <param name="resultadoPrueba">resultadoPrueba que se desea evaluar</param>
        /// <param name="prueba">Prueba que se desea evaluar</param>
        /// <returns>resultadoMetodoPuntos encontrado</returns>
        public AResultadoMetodoCalificacion Calcular(IResultadoPrueba resultadoPrueba, APrueba prueba)
        {
            if (resultadoPrueba == null || (resultadoPrueba as ResultadoPruebaDinamica) == null) throw new ArgumentException("MetodoPuntos:el resultado prueba nulo o diferente a PruebaDinamica", "resultadoPrueba");
            if (prueba == null || (prueba as PruebaDinamica) == null) throw new ArgumentException("MetodoPuntos:prueba nulo o diferente a PruebaDinamica", "prueba");

            PruebaDinamica pruebaDinamica = prueba as PruebaDinamica;
            ResultadoPruebaDinamica resultadoPruebaDinamca = resultadoPrueba as ResultadoPruebaDinamica;
            RegistroPruebaDinamica registroPruebaDinamica = resultadoPrueba.RegistroPrueba as RegistroPruebaDinamica;

            List<APuntaje> listaPuntajes = pruebaDinamica.ListaPuntajes.ToList();

            ResultadoMetodoPuntos resultadoMetodoPuntos = null;

            if ((pruebaDinamica.Modelo as ModeloDinamico).MetodoCalificacion != null)
            {
                if ((pruebaDinamica.Modelo as ModeloDinamico).MetodoCalificacion == EMetodoCalificacion.PUNTOS)
                {
                    resultadoMetodoPuntos = new ResultadoMetodoPuntos();

                    decimal puntosObtenidos = registroPruebaDinamica.CalcularPuntajePrueba();
                    resultadoMetodoPuntos.Puntos = puntosObtenidos;
                    decimal puntajeTotal = registroPruebaDinamica.CalcularPuntajePosiblePrueba();
                    resultadoMetodoPuntos.MaximoPuntos = puntajeTotal;

                    resultadoMetodoPuntos.EscalaPuntajeDinamica = this.ObtenerClasificador(listaPuntajes, puntosObtenidos);
                    resultadoMetodoPuntos.EsAproximado = false;

                    if (resultadoMetodoPuntos.EscalaPuntajeDinamica == null)
                    {
                        resultadoMetodoPuntos.EscalaPuntajeDinamica = this.ObtenerClasificadorAproximado(listaPuntajes, puntosObtenidos);
                        resultadoMetodoPuntos.EsAproximado = true;
                    }
                }
                else
                {
                    throw new ArgumentException("El metodo de calificación de la prueba es dierente a metodo de calificación por puntos");
                }
            }
            return resultadoMetodoPuntos;
        }


        /// <summary>
        /// Obtiene el clasificador correspondiente al resultado de la prueba
        /// </summary>
        /// <param name="listaEscalaPuntajeDinamica">Provee la lista de rangos configurados</param>
        /// <param name="puntos">puntos obtenidos de la prueba</param>
        /// <returns>EscalaPuntajeDinamica</returns>
        private EscalaPuntajeDinamica ObtenerClasificador(List<APuntaje> listaEscalaPuntajeDinamica, decimal puntos)
        {
            EscalaPuntajeDinamica escalaPuntajeDinamica = null;

            if (listaEscalaPuntajeDinamica != null && listaEscalaPuntajeDinamica.Count > 0)
            {
                listaEscalaPuntajeDinamica = listaEscalaPuntajeDinamica.OrderBy(item => item.PuntajeMinimo).ToList();
                int i = 0;
                for (i = 0; i < listaEscalaPuntajeDinamica.Count; i++)
                {
                    if (puntos >= listaEscalaPuntajeDinamica[i].PuntajeMinimo && puntos < listaEscalaPuntajeDinamica[i].PuntajeMaximo)
                    {
                        escalaPuntajeDinamica = new EscalaPuntajeDinamica();
                        escalaPuntajeDinamica = listaEscalaPuntajeDinamica[i] as EscalaPuntajeDinamica;
                        break;
                    }
                    if (listaEscalaPuntajeDinamica.Count == i+1)
                    {
                        if (puntos == listaEscalaPuntajeDinamica[i].PuntajeMaximo)
                        {
                            escalaPuntajeDinamica = listaEscalaPuntajeDinamica[i] as EscalaPuntajeDinamica;
                        }
                    }
                }
            }

            return escalaPuntajeDinamica;
        }

        /// <summary>
        /// Obtiene un clasificador Aproximado de la prueba
        /// </summary>
        /// <param name="listaEscalas"> Provee la lista de rangos configurados</param>
        /// <param name="puntos">puntaje obtenido de la prueba</param>
        /// <returns>EscalaPuntaje</returns>
        private EscalaPuntajeDinamica ObtenerClasificadorAproximado(List<APuntaje> listaEscalas, decimal puntos)
        {
            EscalaPuntajeDinamica escalaPuntaje = null;

            if (listaEscalas != null && listaEscalas.Count > 0)
            {
                listaEscalas = listaEscalas.OrderBy(item => item.PuntajeMinimo).ToList();
                int i = 0;
                for (i = 0; i < listaEscalas.Count; i++)
                {
                    if (puntos <= listaEscalas[i].PuntajeMinimo)
                    {
                        escalaPuntaje = new EscalaPuntajeDinamica();
                        escalaPuntaje = listaEscalas[i] as EscalaPuntajeDinamica;
                        break;
                    }
                }

                if (escalaPuntaje == null)
                {
                    escalaPuntaje = listaEscalas[listaEscalas.Count - 1] as EscalaPuntajeDinamica;
                }
            }
            return escalaPuntaje;
        }
    }
}
