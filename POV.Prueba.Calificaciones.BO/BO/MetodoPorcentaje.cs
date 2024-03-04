using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using POV.Modelo.BO;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Calificaciones.BO { 
   /// <summary>
   /// Metodo Porcentaje
   /// </summary>
    public class MetodoPorcentaje : IPoliticaCalificacion
    { 
        public AResultadoMetodoCalificacion Calcular(IResultadoPrueba resultadoPrueba,APrueba prueba)
        {
            if (resultadoPrueba == null || (resultadoPrueba as ResultadoPruebaDinamica) == null) throw new ArgumentException("MetodoPuntos:el resultado prueba nulo o diferente a PruebaDinamica", "resultadoPrueba");
            if (prueba == null || (prueba as PruebaDinamica) == null) throw new ArgumentException("MetodoPuntos:prueba nulo o diferente a PruebaDinamica", "prueba");
            ResultadoMetodoPorcentaje resultadoMetodoPorcentaje = new ResultadoMetodoPorcentaje();
            ResultadoPruebaDinamica resultadoPruebaDinamica = new ResultadoPruebaDinamica();
            resultadoPruebaDinamica  = (resultadoPrueba as ResultadoPruebaDinamica);
            int[] n = ObtenerAciertos(resultadoPruebaDinamica);
            decimal porcentaje = ((Decimal)n[0] / (Decimal)n[1]) * 100;
            resultadoMetodoPorcentaje = ObtenerResultadoMetodo(prueba, porcentaje);
            resultadoMetodoPorcentaje.ResultadoPrueba = resultadoPruebaDinamica;
            resultadoMetodoPorcentaje.NumeroAciertos = n[0];
            resultadoMetodoPorcentaje.TotalAciertos = n[1];
            return resultadoMetodoPorcentaje;
        }
        /// <summary>
        /// Obtiene los aciertos contestados y el total de los aciertos posibles
        /// </summary>
        /// <param name="resultadoPruebaDinamica">ResultadoPruebaDinamica: provee la prueba a evaluar </param>
        /// <returns>int[] determina los aciertos contestados y el numero de aciertos posibles </returns>
        private int[] ObtenerAciertos(ResultadoPruebaDinamica resultadoPruebaDinamica)
        {
            int[] resultados = new int[2];
            foreach (RespuestaReactivoDinamica rrd in resultadoPruebaDinamica.RegistroPrueba.ListaRespuestaReactivos)
            {
                foreach(RespuestaPreguntaDinamica rpd in rrd.ListaRespuestaPreguntas)
                {
                    if (rpd.CalificarRespuestaPreguntaDinamica() > 0)
                        resultados[0]++;
                    resultados[1]++;
                }
            }
            return resultados;
        }
        /// <summary>
        /// Obtiene el ResultadoMetodoPorcentaje que corresponde a la puntuacion por porcentaje
       /// </summary>
       /// <param name="prueba">APrueba: provee la prueba a evaluar </param>
       /// <param name="porentaje">decimal: provee el resultado de los aciertos </param>
        /// <returns>ResultadoMetodoPorcentaje determina el resultadometodoporcentaje con base en el resultado del alumno </returns>
        private ResultadoMetodoPorcentaje ObtenerResultadoMetodo(APrueba pruebaDinamica, decimal porentaje)
        {
            ResultadoMetodoPorcentaje resultadoMetodoPorcentaje = new ResultadoMetodoPorcentaje();
            List<APuntaje> listaEscalasDinamicas = pruebaDinamica.ListaPuntajes.ToList();
            listaEscalasDinamicas = listaEscalasDinamicas.OrderBy(item => item.PuntajeMinimo).ToList();
            int i = 0;
            for (i = 0; i < listaEscalasDinamicas.Count; i++ )
            {
                if (listaEscalasDinamicas[i].PuntajeMinimo <= porentaje && porentaje < listaEscalasDinamicas[i].PuntajeMaximo)
                {
                    resultadoMetodoPorcentaje.EscalaPorcentajeDinamica = listaEscalasDinamicas[i] as EscalaPorcentajeDinamica;
                    resultadoMetodoPorcentaje.EsAproximado = false;
                    return resultadoMetodoPorcentaje;
                }
                if (porentaje < listaEscalasDinamicas[i].PuntajeMinimo)
                {
                    resultadoMetodoPorcentaje.EscalaPorcentajeDinamica = listaEscalasDinamicas[i] as EscalaPorcentajeDinamica;
                    resultadoMetodoPorcentaje.EsAproximado = true;
                    return resultadoMetodoPorcentaje;
                }
            }
            resultadoMetodoPorcentaje.EscalaPorcentajeDinamica = listaEscalasDinamicas[i-1] as EscalaPorcentajeDinamica;
            if (porentaje <= listaEscalasDinamicas[i - 1].PuntajeMaximo)
                resultadoMetodoPorcentaje.EsAproximado = false;
            else
                resultadoMetodoPorcentaje.EsAproximado = true;
            return resultadoMetodoPorcentaje;
        }
   } 
}
