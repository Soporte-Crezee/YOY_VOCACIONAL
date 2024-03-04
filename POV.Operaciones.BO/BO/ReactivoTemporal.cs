using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Reactivos.BO;

namespace POV.Operaciones.BO
{
    public class ReactivoTemporal
    {
        public Guid newVersionID { get; set; }

        public Reactivo Reactivo { get; set; }

        public List<EvaluacionRango> Rangos { get; set; }

        public List<PreguntaTemporal> Preguntas { get; set; }

        public int NumeroBloques { get; set; }

        public int NumeroBloquesCapturados { get; set; }

        public int NumeroBloqueActual { get; set; }

        public bool ReactivoComplete { get; set; }

        public bool BloquesComplete { get; set; }

        public bool EvaluacionComplete { get; set; }

        /// <summary>
        /// Devuelve una lista con tres rangos de evaluacion inicializados;
        /// </summary>
        /// <returns></returns>
        public List<EvaluacionRango> GetListNewEvaluacionRango()
        {
            List<EvaluacionRango> rangos = new List<EvaluacionRango>();

            EvaluacionRango rango1 = new EvaluacionRango();
      
            rango1.EvaluacionID = Guid.NewGuid();
            rango1.Inicio = 0;
            rango1.Fin = 7;
            rango1.PorcentajeCalificacion = (decimal)0.25;

            rangos.Add(rango1);

            EvaluacionRango rango2 = new EvaluacionRango();
            rango2.EvaluacionID = Guid.NewGuid();
            rango2.Inicio = 7;
            rango2.Fin = 10;
            rango2.PorcentajeCalificacion = (decimal)0.125;

            rangos.Add(rango2);

            EvaluacionRango rango3 = new EvaluacionRango();
            rango3.EvaluacionID = Guid.NewGuid();
            rango3.Inicio = 10;
            rango3.Fin = 11;
            rango3.PorcentajeCalificacion = 0;

            rangos.Add(rango3);

            return rangos;
        }

        /// <summary>
        /// Construye y devuelve una lista de bloques de pregunta dependiendo del numero que recibe
        /// </summary>
        /// <param name="numBloques"></param>
        /// <returns></returns>
        public List<PreguntaTemporal> GetListNewBloquesPregunta(int numBloques)
        {
            List<PreguntaTemporal> preguntas = new List<PreguntaTemporal>();

            for (int index = 1; index <= numBloques; index++)
            {
                PreguntaTemporal preguntaTemporal = new PreguntaTemporal();
                preguntaTemporal.Orden = index - 1;
                preguntaTemporal.Pregunta = preguntaTemporal.GetNewPregunta();
                preguntas.Add(preguntaTemporal);
            }
            return preguntas;
        }
    }
}
