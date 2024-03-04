using System;
using System.Collections.Generic;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Expediente.BO;
using System.Linq;
using POV.Modelo.BO;
namespace POV.Prueba.Calificaciones.BO { 
   /// <summary>
   /// Califica una prueba por el método de calificación por clasificadores.
   /// </summary>
   public class MetodoSeleccion: IPoliticaCalificacion {
       /// <summary>
       /// Calcula el resultado de una prueba a partir del método de selección de clasificadores.
       /// </summary>
       /// <param name="resultadoPrueba">Resultado de la prueba que contiene los reactivos y preguntas contestados</param>
       /// <param name="prueba">La prueba que contiene la estructura del modelo de la prueba</param>
       /// <returns>El resultado del método de calificación, si el método return null, quiere decir que el método de calificacion está mal 
       /// mal configurado, no tiene puntajes, escalas,  o clasificadores
       /// </returns>
       public AResultadoMetodoCalificacion Calcular(IResultadoPrueba resultadoPrueba, APrueba prueba)
       {
         
           ResultadoMetodoSeleccion resultado = new ResultadoMetodoSeleccion();
           resultado.ListaDetalleResultadoSeleccion = new List<DetalleResultadoSeleccion>();

           if (prueba == null)
               return null;
           if (prueba.ListaPuntajes == null)
               return null;
           if (!prueba.ListaPuntajes.Any()) {
               return null;
           }
           IEnumerable<APuntaje> puntajesConfigurados=  prueba.ListaPuntajes;
           IDictionary<int, decimal> result = null;
           try
           {
                result = this.CalcularResultadoMetodoSeleccion(resultadoPrueba, prueba);
           }
           catch (Exception e) {
               throw e;
           }
           if (result == null)
               return null;
           foreach (KeyValuePair<int, decimal> elementoValor in result) {
               try
               {
                   DetalleResultadoSeleccion detalle = ObtenerDetalleResultadoSeleccion(elementoValor, prueba, resultadoPrueba);
                   if (detalle == null)
                       return null;
                   resultado.ListaDetalleResultadoSeleccion.Add(detalle);
               }
               catch (Exception e) { throw e; }
           }

           if(puntajesConfigurados!=null){
            Dictionary<int,decimal> puntajesNoConfiguradosValor=   ObternerClasificadoresNoConfigurados(puntajesConfigurados,resultado);

               
            foreach (KeyValuePair<int, decimal> elementoValor in puntajesNoConfiguradosValor)
            {
                try
                {
                    DetalleResultadoSeleccion detalle = ObtenerDetalleResultadoSeleccion(elementoValor, prueba, resultadoPrueba);
                    if (detalle == null)
                        return null;
                    resultado.ListaDetalleResultadoSeleccion.Add(detalle);
                }
                catch (Exception e) { throw e; }
            }

           }


           resultado.ResultadoPrueba = resultadoPrueba as ResultadoPruebaDinamica;
           return resultado;
           
       }

       private Dictionary<int, decimal> ObternerClasificadoresNoConfigurados(IEnumerable<APuntaje> puntajesConfigurados,ResultadoMetodoSeleccion resultado)
       {
           Dictionary<int, decimal> puntajesNoConfiguradosValor = new Dictionary<int, decimal>();
           foreach (APuntaje puntaje in puntajesConfigurados)
           {
               AEscalaDinamica escalaDinamica = puntaje as AEscalaDinamica;
               List<DetalleResultadoSeleccion> resultadoClasificador = null;
               if (escalaDinamica != null)
               {
                   if (escalaDinamica.Clasificador == null)
                       return null;
                   if (escalaDinamica.Clasificador.ClasificadorID == null)
                       return null;

                   resultadoClasificador = resultado.ListaDetalleResultadoSeleccion.Where(x => x.EscalaSeleccionDinamica.Clasificador.ClasificadorID == escalaDinamica.Clasificador.ClasificadorID).ToList<DetalleResultadoSeleccion>();

                   if (resultadoClasificador.Count <= 0)
                   {
                       if (!puntajesNoConfiguradosValor.ContainsKey(escalaDinamica.Clasificador.ClasificadorID.Value))
                           puntajesNoConfiguradosValor.Add(escalaDinamica.Clasificador.ClasificadorID.Value, 0);
                   }
               }
           }
           return puntajesNoConfiguradosValor;
       }
       private DetalleResultadoSeleccion ObtenerDetalleResultadoSeleccion(KeyValuePair<int, decimal> elemento, APrueba prueba, IResultadoPrueba resultadoPrueba)
       {
     
           AResultadoMetodoCalificacion resultado = new ResultadoMetodoSeleccion();
           ResultadoMetodoSeleccion result = resultado.EncontrarEscalaFromValor(prueba, elemento.Value,elemento.Key) as ResultadoMetodoSeleccion;
           if (result == null)
               return null;
           if (result.ListaDetalleResultadoSeleccion == null)
               return null;
           if (result.ListaDetalleResultadoSeleccion.Count <= 0)
               return null;

           return result.ListaDetalleResultadoSeleccion[0];
           
       }

       private IDictionary<int,decimal> CalcularResultadoMetodoSeleccion(IResultadoPrueba resultadoPrueba, APrueba prueba)
       {
           if(resultadoPrueba==null || (resultadoPrueba as ResultadoPruebaDinamica)==null) throw new ArgumentException("MetodoSeleccion:el resultado prueba nulo o diferente a Prueba Dinámica","resultadoPrueba");     
           if(prueba==null || (prueba as PruebaDinamica)==null) throw new ArgumentException("Metodo por Seleccion de clasificadores:prueba nulo o diferente a PruebaDinámica","prueba");


           ResultadoMetodoSeleccion resultado = new ResultadoMetodoSeleccion();
           ARegistroPrueba registroPrueba = resultadoPrueba.RegistroPrueba;

           if(registroPrueba==null)
               throw new Exception("No existe un registro prueba");    
           IDictionary<int,decimal> result =null;
           if (registroPrueba is RegistroPruebaDinamica)
           {
               RegistroPruebaDinamica registroPruebaDinamica = registroPrueba as RegistroPruebaDinamica;

               if (registroPrueba.GetEstadoPruebaActual() != EEstadoPrueba.CERRADA)
                   throw new Exception("MetodoSeleccion CalcularResultadoMetodoSeleccion, Prueba no ha sido cerrada");

               try
               {
                   result = registroPruebaDinamica.ResultadoMetodoCalificacionSeleccionClasificadores();
               }
               catch (Exception e) { throw e; };
           }
           else throw new Exception("El tipo de prueba, no es la prueba esperada");    

           return result;
       }
  
       }
       
}
