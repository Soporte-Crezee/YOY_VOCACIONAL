using System;
using System.Collections.Generic;
using System.Text;
using POV.Reactivos.BO;

namespace POV.ReactivosUsuario.BO { 
   /// <summary>
   /// RespuestaPreguntaUsuario
   /// </summary>
   public class RespuestaPreguntaUsuario { 
      private Guid? respuestaPreguntaUsuarioID;
      /// <summary>
      /// identificador de la respuesta pregunta usuario
      /// </summary>
      public Guid? RespuestaPreguntaUsuarioID{
         get{ return this.respuestaPreguntaUsuarioID; }
         set{ this.respuestaPreguntaUsuarioID = value; }
      }
      private Pregunta pregunta;
      /// <summary>
      /// pregunta
      /// </summary>
      public Pregunta Pregunta{
         get{ return this.pregunta; }
         set{ this.pregunta = value; }
      }
      private RespuestaUsuario respuestaUsuario;
      /// <summary>
      /// opcion de la respuesta usuario
      /// </summary>
      public RespuestaUsuario RespuestaUsuario{
         get{ return this.respuestaUsuario; }
         set{ this.respuestaUsuario = value; }
      }
   } 
}
