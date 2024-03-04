using System;
using System.Collections.Generic;
using System.Text;
using POV.Reactivos.BO;

namespace POV.ReactivosUsuario.BO { 
   /// <summary>
   /// RespuestaUsuarioOpcionMultiple
   /// </summary>
   public class RespuestaUsuarioOpcionMultiple: RespuestaUsuario { 
      private OpcionRespuestaPlantilla opcionRespuestaPlantilla;
      /// <summary>
      /// opcion de la respuesta usuario
      /// </summary>
      public OpcionRespuestaPlantilla OpcionRespuestaPlantilla{
         get{ return this.opcionRespuestaPlantilla; }
         set{ this.opcionRespuestaPlantilla = value; }
      }
   } 
}
