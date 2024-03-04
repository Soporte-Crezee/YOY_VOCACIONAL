using System;
using POV.Comun.BO;

namespace POV.CentroEducativo.BO { 
   /// <summary>
   /// Clase para el objeto Grupo
   /// </summary>
   public class Grupo : ICloneable, IObservableChange { 
      private Guid? grupoID;
      /// <summary>
      /// Identificador del grupo
      /// </summary>
      public Guid? GrupoID{
         get{ return this.grupoID; }
         set
         {
             if (this.grupoID != value)
             {
                 this.grupoID = value;
                 this.Cambio();
             }
         }
      }
      private string nombre;
      /// <summary>
      /// Nombre del grupo
      /// </summary>
      public string Nombre{
         get{ return this.nombre; }
         set
         {
             if (this.Nombre != value)
             {
                 this.nombre = value;
                 this.Cambio();
             }
         }
      }
      private byte? grado;
      /// <summary>
      /// Grado del grupo
      /// </summary>
      public byte? Grado{
         get{ return this.grado; }
         set
         {
             if (this.grado != value)
             {
                 this.grado = value;
                 this.Cambio();
             }
         }
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
       private void Cambio()
       {
           if (this.observer != null)
               this.observer.Cambio();
       }
   } 
}
