using System;
using System.Collections.Generic;
using System.Text;

namespace GP.SocialEngine.BO { 
   /// <summary>
   /// Notificacion
   /// </summary>
   public class Notificacion: ICloneable { 
      private Guid? notificacionID;
      /// <summary>
      /// Identificador de la notificacion.
      /// </summary>
      public Guid? NotificacionID{
         get{ return this.notificacionID; }
         set{ this.notificacionID = value; }
      }
      private DateTime? fechaRegistro;
      /// <summary>
      /// Fecha de registro de la notificacion
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
      private UsuarioSocial emisor;
      /// <summary>
      /// Usuario social emisor de la notificacion
      /// </summary>
      public UsuarioSocial Emisor{
         get{ return this.emisor; }
         set{ this.emisor = value; }
      }
      private UsuarioSocial receptor;
      /// <summary>
      /// Usuario social receptor de la notificacion
      /// </summary>
      public UsuarioSocial Receptor{
         get{ return this.receptor; }
         set{ this.receptor = value; }
      }
      private INotificable notificable;
      /// <summary>
      /// Elemento notificable
      /// </summary>
      public INotificable Notificable{
         get{ return this.notificable; }
         set{ this.notificable = value; }
      }
      private ETipoNotificacion? tipoNotificacion;
      /// <summary>
      /// Tipo de notificacion
      /// </summary>
      public ETipoNotificacion? TipoNotificacion{
         get{ return this.tipoNotificacion; }
         set{ this.tipoNotificacion = value; }
      }
      private EEstatusNotificacion? estatusNotificacion;
      /// <summary>
      /// Estatus de la notificacion
      /// </summary>
      public EEstatusNotificacion? EstatusNotificacion{
         get{ return this.estatusNotificacion; }
         set{ this.estatusNotificacion = value; }
      }


      public short? ToShortEstatusNotificacion
      {
          get { return (short)this.estatusNotificacion; }
          set { this.estatusNotificacion = (EEstatusNotificacion)value; }

      }

      public short? ToShortTipoNotificacion
      {
          get { return (short)this.tipoNotificacion; }
          set { this.tipoNotificacion = (ETipoNotificacion)value; }
      }

      public object Clone()
      {
          return this.MemberwiseClone();
      }
   } 
}
