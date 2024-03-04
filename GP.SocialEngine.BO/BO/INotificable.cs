using System;
using System.Collections.Generic;
using System.Text;

namespace GP.SocialEngine.BO { 
   /// <summary>
   /// Interfaz Notificable
   /// </summary>
   public interface INotificable { 
      /// <summary>
      /// Identificador del elemento notificable
      /// </summary>
      Guid? GUID{
         get;
         set;
      }
      /// <summary>
      /// Texto de la notificacion
      /// </summary>
      string TextoNotificacion{
         get;
         set;
      }
      /// <summary>
      /// URL de referencia del elemento notificable
      /// </summary>
      string URLReferencia{
         get;
         set;
      }
   } 
}
