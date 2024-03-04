using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Reactivos.BO { 
   /// <summary>
   /// RespuestaPlantilla
   /// </summary>
   public abstract class RespuestaPlantilla:ICloneable { 
      protected int? respuestaPlantillaID;
      /// <summary>
      /// identificador de la RespuestaPlantilla
      /// </summary>
      public int? RespuestaPlantillaID{
         get{ return this.respuestaPlantillaID; }
         set{ this.respuestaPlantillaID = value; }
      }
      protected ETipoRespuestaPlantilla? tipoRespuestaPlantilla;
      /// <summary>
      /// Tipo de plantilla de respuestas
      /// </summary>
      public ETipoRespuestaPlantilla? TipoRespuestaPlantilla{
         get{ return this.tipoRespuestaPlantilla; }
         set{ this.tipoRespuestaPlantilla = value; }
      }

      public short? ToShortTipoRespuestaPlantilla
      {
          get { return (short)this.tipoRespuestaPlantilla; }
          set { this.tipoRespuestaPlantilla = (ETipoRespuestaPlantilla)value; }
      }
      protected bool? estatus;
      /// <summary>
      /// estatus de la RespuestaPlantilla
      /// </summary>
      public bool? Estatus{
         get{ return this.estatus; }
         set{ this.estatus = value; }
      }
      protected DateTime? fechaRegistro;
      /// <summary>
      /// Fecha de Registro de la RespuestaPlantilla
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }

       /// <summary>
       /// TipoPuntaje RespuestaPlantilla
       /// </summary>
      private ETipoPuntaje? tipoPuntaje;
       public ETipoPuntaje? TipoPuntaje
       {
           get { return this.tipoPuntaje; }
           set { this.tipoPuntaje = value; }
       }
      public virtual object Clone()
      {
          return this.MemberwiseClone();
        
      }
   } 
}
