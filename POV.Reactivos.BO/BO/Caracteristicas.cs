
using System;

namespace POV.Reactivos.BO { 
   /// <summary>
   /// Caracteristicas del reactivo
   /// </summary>
    public abstract class Caracteristicas : ICloneable
   {

       public virtual object Clone()
       {
          return this.MemberwiseClone();
       }
   } 
}
