using System;
using System.Collections.Generic;
using System.Text;

namespace GP.SocialEngine.BO { 
   /// <summary>
   /// Interfaz Ireportable
   /// </summary>
   public interface IReportable { 
      /// <summary>
      /// Identificador del elemento reportable
      /// </summary>
      Guid? GUID{
         get;
         set;
      }
      /// <summary>
      /// Estado del elemento reportado
      /// </summary>
      bool? Estatus{
         get;
         set;
      }
   } 
}
