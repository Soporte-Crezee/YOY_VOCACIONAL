using System;
using POV.Comun.BO;

namespace POV.Profesionalizacion.BO { 
   /// <summary>
   /// Situación de Aprendizaje
   /// </summary>
   public class SituacionAprendizaje:ICloneable,IObservableChange { 
      private long? situacionAprendizajeID;
      /// <summary>
      /// Identificador unico
      /// </summary>
      public long? SituacionAprendizajeID{
         get{ return this.situacionAprendizajeID; }
         set{ this.situacionAprendizajeID = value; }
      }
      private string nombre;
      /// <summary>
      /// Nombre
      /// </summary>
      public string Nombre{
         get{ return this.nombre; }
         set{ this.nombre = value; }
      }


      private string descripcion;
      /// <summary>
      /// Nombre
      /// </summary>
      public string Descripcion
      {
          get { return this.descripcion; }
          set { this.descripcion = value; }
      }

      private DateTime? fechaRegistro;
      /// <summary>
      /// Fecha Alta
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
      private EEstatusProfesionalizacion? estatusProfesionalizacion;
      /// <summary>
      /// EEstatusProfesionalizacion
      /// </summary>
      public EEstatusProfesionalizacion? EstatusProfesionalizacion{
         get{ return this.estatusProfesionalizacion; }
         set{ this.estatusProfesionalizacion = value; }
      }

      private AAgrupadorContenidoDigital agrupadorContenidoDigital;
      /// <summary>
      /// AAGrupadorContenidoDigital
      /// </summary>
      public AAgrupadorContenidoDigital AgrupadorContenidoDigital
      {
         get{ return this.agrupadorContenidoDigital; }
         set{ this.agrupadorContenidoDigital = value; }
      }

      public object Clone()
      {
          return this.MemberwiseClone();
      }
      public object CloneAll()
      {
          return this.Clone();
      }

       private IObserverChange observer;

      public void Registrar(IObserverChange observer)
      {
           this.observer = observer;
      }
      public void Cambio()
      {
          if(this.observer !=null)
              this.observer.Cambio();
         
      }
   } 
}
