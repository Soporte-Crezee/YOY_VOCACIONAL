using System;
using System.Collections.Generic;
using System.Text;

namespace POV.ReactivosUsuario.BO { 
   /// <summary>
   /// RespuestaUsuario
   /// </summary>
   public class RespuestaUsuario { 
      private DateTime? fechaRegistro;
      /// <summary>
      /// fecha de registro de la respuesta usuario
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
      private ETipoRespuestaUsuario? tipoRespuestaUsuario;
      /// <summary>
      /// TipoRespuestaUsuario de la  RespuestaUsuario
      /// </summary>
      public ETipoRespuestaUsuario? TipoRespuestaUsuario{
         get{ return this.tipoRespuestaUsuario; }
         set{ this.tipoRespuestaUsuario = value; }
      }

      public short? ToShortTipoRespuestaUsuario
      {
          get { return (short)this.tipoRespuestaUsuario; }
          set { this.tipoRespuestaUsuario = (ETipoRespuestaUsuario)value; }
      }
   } 
}
