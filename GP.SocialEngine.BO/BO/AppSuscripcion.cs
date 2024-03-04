using System;
using System.Collections.Generic;
using System.Text;
using GP.SocialEngine.Interfaces;

namespace GP.SocialEngine.BO { 
   /// <summary>
   /// Suscripcion a la red
   /// </summary>
   public class AppSuscripcion { 
      private long? appSuscripcionID;
      /// <summary>
      /// Identificador autonumérico del AppSuscripcion
      /// </summary>
      public long? AppSuscripcionID{
         get{ return this.appSuscripcionID; }
         set{ this.appSuscripcionID = value; }
      }
      private DateTime? fechaRegistro;
      /// <summary>
      /// Fecha de la suscripción
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
      private bool? estatus;
      /// <summary>
      /// Estatus de la suscripcion
      /// </summary>
      public bool? Estatus{
         get{ return this.estatus; }
         set{ this.estatus = value; }
      }
      private IAppSocial appSocial;
      /// <summary>
      /// IAppSocial
      /// </summary>
      public IAppSocial AppSocial{
         get{ return this.appSocial; }
         set{ this.appSocial = value; }
      }

      private EAppType? appType;
      /// <summary>
      /// IAppSocial
      /// </summary>
      public EAppType? AppType
      {
          get { return this.appType; }
          set { this.appType = value; }
      }

      public short? ToShortAppType
      {
          get { return (short)this.appType; }
          set { this.appType = (EAppType)value; }
      }
   } 
}
