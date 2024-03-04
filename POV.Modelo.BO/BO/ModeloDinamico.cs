using System;
using System.Collections.Generic;
using System.Text;
using POV.Modelo.BO;

namespace POV.Modelo.BO { 
   /// <summary>
   /// Modelo de prueba dinámico
   /// </summary>
   public class ModeloDinamico: AModelo { 
      private EMetodoCalificacion? metodoCalificacion;
      /// <summary>
      /// Método de Calificación del Modelo
      /// </summary>
      public EMetodoCalificacion? MetodoCalificacion{
         get{ return this.metodoCalificacion; }
         set{ this.metodoCalificacion = value; }
      }
      private List<PropiedadPersonalizada> listaPropiedadPersonalizada;
      /// <summary>
      /// lista de Propiedades del Modelo
      /// </summary>
      public List<PropiedadPersonalizada> ListaPropiedadPersonalizada{
         get{ return this.listaPropiedadPersonalizada; }
         set{ this.listaPropiedadPersonalizada = value; }
      }
      private List<Clasificador> listaClasificador;
      /// <summary>
      /// lista de Clasificadores del Modelo
      /// </summary>
      public List<Clasificador> ListaClasificador{
         get{ return this.listaClasificador; }
         set{ this.listaClasificador = value; }
      }

      public override ETipoModelo? TipoModelo
      {
          get { return ETipoModelo.Dinamico; }
      }
       /// <summary>
       /// Nombre del metodo de calificacion
       /// </summary>
      public string NombreMetodoCalificacion
      {
          get
          {
              string nombre = string.Empty;

              if (metodoCalificacion != null)
              {
                  switch (metodoCalificacion)
                  {
                      case EMetodoCalificacion.PUNTOS:
                          return POV.Modelo.Properties.Resources.PUNTOS;
                      case EMetodoCalificacion.PORCENTAJE:
                          return POV.Modelo.Properties.Resources.PORCENTAJE;
                      case EMetodoCalificacion.CLASIFICACION:
                          return POV.Modelo.Properties.Resources.CLASIFICACION;
                      case EMetodoCalificacion.SELECCION:
                          return POV.Modelo.Properties.Resources.SELECCION;
                  }
              }


              return nombre;
          }
      }
   } 
}
