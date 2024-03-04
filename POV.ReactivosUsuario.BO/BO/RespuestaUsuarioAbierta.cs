using System;
using System.Collections.Generic;
using System.Text;
using POV.Reactivos.BO;

namespace POV.ReactivosUsuario.BO { 
   /// <summary>
   /// RespuestaUsuarioAbierta
   /// </summary>
   public class RespuestaUsuarioAbierta: RespuestaUsuario { 
      private string textoRespuesta;
      /// <summary>
      /// texto de la respuesta usuario
      /// </summary>
      public string TextoRespuesta{
         get{ return this.textoRespuesta; }
         set{ this.textoRespuesta = value; }
      }
   } 
}
