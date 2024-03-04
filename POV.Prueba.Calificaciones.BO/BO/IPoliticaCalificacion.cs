using System;
using System.Collections.Generic;
using POV.Prueba.BO;
namespace POV.Prueba.Calificaciones.BO { 
   /// <summary>
   /// IPoliticaCalificacion
   /// </summary>
   public interface IPoliticaCalificacion
   {
       /// <summary>
       /// Calcula la calificación de una prueba
       /// </summary>
       AResultadoMetodoCalificacion Calcular(IResultadoPrueba resultadoPrueba, APrueba prueba);
   } 
}
