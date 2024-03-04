using System;
using System.Collections.Generic;

namespace GP.SocialEngine.BO { 
   /// <summary>
   /// Consulta de Mensaje Privado
   /// </summary>
   public class Mensaje: INotificable { 
      private Guid? mensajeID;
      /// <summary>
      /// Identificador unico
      /// </summary>
      public Guid? MensajeID{
         get{ return this.mensajeID; }
         set{ this.mensajeID = value; }
      }
      private string guidConversacion;
      /// <summary>
      /// Guid para Identificar la conversacion
      /// </summary>
      public string GuidConversacion{
         get{ return this.guidConversacion; }
         set{ this.guidConversacion = value; }
      }
      private string contenido;
      /// <summary>
      /// Contenido del Mensaje
      /// </summary>
      public string Contenido{
         get{ return this.contenido; }
         set{ this.contenido = value; }
      }
      private DateTime? fechaMensaje;
      /// <summary>
      /// Fecha Alta Mensaje
      /// </summary>
      public DateTime? FechaMensaje{
         get{ return this.fechaMensaje; }
         set{ this.fechaMensaje = value; }
      }
      private List<UsuarioSocial> destinatarios;
      /// <summary>
      /// Destinatario del Mensaje
      /// </summary>
      public List<UsuarioSocial> Destinatarios{
         get{ return this.destinatarios; }
         set{ this.destinatarios = value; }
      }
      private UsuarioSocial remitente;
      /// <summary>
      /// Remitente del Mensaje
      /// </summary>
      public UsuarioSocial Remitente{
         get{ return this.remitente; }
         set{ this.remitente = value; }
      }
      private bool? estatus;
      /// <summary>
      /// Estado del Mensaje
      /// </summary>
      public bool? Estatus{
         get{ return this.estatus; }
         set{ this.estatus = value; }
      }
      private string asunto;
      /// <summary>
      /// Asunto del Mensaje
      /// </summary>
      public string Asunto{
         get{ return this.asunto; }
         set{ this.asunto = value; }
      }
     
      /// <summary>
      /// Guid de Identificación
      /// </summary>
      public Guid? GUID{
         get{ return this.MensajeID; }
         set{ this.MensajeID = value; }
      }

       public string textoNotificacion;
      public string TextoNotificacion
      {
          get { return GP.SocialEngine.Properties.Resources.MensajeNotificacion; }
          set { throw new NotImplementedException(); }
      }

      /// <summary>
      /// URLReferencia
      /// </summary>
      public string URLReferencia{
          get { return "Mensajes.aspx"; }
         set{ throw  new NotImplementedException();}
      }
   } 
}
