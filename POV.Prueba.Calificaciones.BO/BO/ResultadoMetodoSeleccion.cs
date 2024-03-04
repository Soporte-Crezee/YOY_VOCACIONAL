using System;
using System.Collections.Generic;
using System.Linq;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Calificaciones.BO { 
   /// <summary>
   /// Consulta de ResultadoMetodoEstandarizado
   /// </summary>
   public class ResultadoMetodoSeleccion: AResultadoMetodoCalificacion { 
      private List<DetalleResultadoSeleccion> listaDetalleResultadoSeleccion;
      /// <summary>
      /// Lista DetalleResultadoEstandarizado
      /// </summary>
      public List<DetalleResultadoSeleccion> ListaDetalleResultadoSeleccion{
         get{ return this.listaDetalleResultadoSeleccion; }
         set{ this.listaDetalleResultadoSeleccion = value; }
      }

       /// <summary>
       /// Tipo de resultado del método de selección
       /// </summary>

      public override ETipoResultadoMetodo TipoResultadoMetodo
      {
          get { return ETipoResultadoMetodo.SELECCION;}
      }
       /// <summary>
       /// Calcula los resultados predominantes del método de calificacion
       /// </summary>
       /// <returns>La lista de resultados predominantes</returns>
      public List<DetalleResultadoSeleccion> CalcularResultadosCalificacionPredominante() {


          List<DetalleResultadoSeleccion> listaCurrentDetallesSeleccion = this.listaDetalleResultadoSeleccion;
          if (listaCurrentDetallesSeleccion == null)
              return listaCurrentDetallesSeleccion;
          List<DetalleResultadoSeleccion> resultFirst= null;
          List<DetalleResultadoSeleccion> resultFinal = new List<DetalleResultadoSeleccion>();
          resultFirst = listaDetalleResultadoSeleccion.Where(z => z.EscalaSeleccionDinamica.EsPredominante==true).ToList<DetalleResultadoSeleccion>();
                    
         if(resultFirst!=null)
         if (resultFirst.Count <= 0) {
             resultFirst = listaCurrentDetallesSeleccion.OrderByDescending(item => item.Valor).ToList<DetalleResultadoSeleccion>();
             decimal mayor = 0;
             bool primeraVez = true;
             foreach (DetalleResultadoSeleccion detalle in resultFirst) {
                 if (primeraVez)
                 {
                     mayor = detalle.Valor.Value;
                     resultFinal.Add(detalle);
                 }
                 else {
                     if (detalle.Valor.Value == mayor)
                     {
                         resultFinal.Add(detalle);
                     }
                     else break;
                 }
                 primeraVez = false;
             }
             return resultFinal;
         }
         return resultFirst;
      }
      /// <summary>
      /// Obtiene el clasificador indicado para la selección del paquete de juegos.
      /// </summary>
      /// <returns>Clasificador</returns>
      public override Modelo.BO.Clasificador ObtenerClasificadorPredominante()
      {
          DetalleResultadoSeleccion detalle = null;
          var ordenAleatorio = new Random(new Object().GetHashCode());
          List<DetalleResultadoSeleccion> list = this.CalcularResultadosCalificacionPredominante();
          if (list == null)
              return null;
          list = list.OrderBy(r => ordenAleatorio.Next()).ToList();
          if (list != null)
          {
              if (list.Count > 0)
              {
                  detalle = list.FirstOrDefault();
                  return detalle.EscalaSeleccionDinamica.Clasificador;
              }
          }
          return null;
      }
   }
}
