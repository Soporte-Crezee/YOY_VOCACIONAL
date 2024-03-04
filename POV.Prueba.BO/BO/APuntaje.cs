using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using POV.Comun.BO;

namespace POV.Prueba.BO { 
   /// <summary>
   /// Representa un puntaje abstracto de una prueba abstracta
   /// </summary>
    public abstract class APuntaje : ICloneable, IObservableChange
    { 
      private int? puntajeID;
      /// <summary>
      /// Identificador del puntaje
      /// </summary>
      public int? PuntajeID{
         get{ return this.puntajeID; }
         set{ this.puntajeID = value; }
      }
      private decimal? puntajeMinimo;
      /// <summary>
      /// PuntajeMinimo
      /// </summary>
      public decimal? PuntajeMinimo{
         get{ return this.puntajeMinimo; }
         set{
			if (this.puntajeMinimo != value)
			{
				this.puntajeMinimo = value;
				this.Cambio();
			}
		 }
      }
      private decimal? puntajeMaximo;
      /// <summary>
      /// PuntajeMaximo
      /// </summary>
      public decimal? PuntajeMaximo{
         get{ return this.puntajeMaximo; }
		 set{
			if (this.puntajeMaximo != value)
			{
				this.puntajeMaximo = value;
				this.Cambio();
			}
		 }
      }
      private bool? esPorcentaje;
      /// <summary>
      /// EsPorcentaje
      /// </summary>
      public bool? EsPorcentaje{
         get{ return this.esPorcentaje; }
		 set{
			if (this.esPorcentaje != value)
			{
				this.esPorcentaje = value;
				this.Cambio();
			}
		 }
      }
      private bool? esPredominante;
      /// <summary>
      /// EsPredominante
      /// </summary>
      public bool? EsPredominante{
         get{ return this.esPredominante; }
		 set{
			if (this.esPredominante != value)
			{
				this.esPredominante = value;
				this.Cambio();
			}
		 }
      }

        /// <summary>
        /// Activo
        /// </summary>
      private bool? activo;
       public bool? Activo
       {
		   get { return this.activo; }
		   set{
			  if (this.activo != value)
			  {
				  this.activo = value;
				  this.Cambio();
			  }
		   }
       }

      private IObserverChange observer;
      public void Registrar(IObserverChange observer)
      {
          this.observer = observer;
      }
      protected void Cambio()
      {
          if (this.observer != null)
              this.observer.Cambio();
      }
      public object Clone()
      {
          return this.MemberwiseClone();
      }

      public object CloneAll()
      {
          return this.Clone();
      }
   } 
}
