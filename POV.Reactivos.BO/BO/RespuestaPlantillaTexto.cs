using System;
using System.Collections.Generic;

namespace POV.Reactivos.BO { 
   /// <summary>
   /// RespuestaPlantillaTexto
   /// </summary>
   public class RespuestaPlantillaTexto: RespuestaPlantillaAbierta { 
      private int? maximoCaracteres;
      /// <summary>
      /// MaximoCaracteres
      /// </summary>
      public int? MaximoCaracteres{
         get{ return this.maximoCaracteres; }
         set{ this.maximoCaracteres = value; }
      }
      private int? minimoCaracteres;
      /// <summary>
      /// MinimoCaracteres
      /// </summary>
      public int? MinimoCaracteres{
         get{ return this.minimoCaracteres; }
         set{ this.minimoCaracteres = value; }
      }
      private bool? esSensibleMayusculaMinuscula;
      /// <summary>
      /// EsSensibleMayusculaMinuscula
      /// </summary>
      public bool? EsSensibleMayusculaMinuscula{
         get{ return this.esSensibleMayusculaMinuscula; }
         set{ this.esSensibleMayusculaMinuscula = value; }
      }
      private bool? esRespuestaCorta;
      /// <summary>
      /// EsRespuestaCorta
      /// </summary>
      public bool? EsRespuestaCorta{
         get{ return this.esRespuestaCorta; }
         set{ this.esRespuestaCorta = value; }
      }
   } 
}
