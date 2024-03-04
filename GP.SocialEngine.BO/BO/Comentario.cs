using System;
using System.Collections.Generic;
using System.Text;
using GP.SocialEngine.Interfaces;

namespace GP.SocialEngine.BO { 
   /// <summary>
   /// Comentario
   /// </summary>
   public class Comentario : INotificable,ICloneable,IReportable { 
      private Guid? comentarioID;
      /// <summary>
      /// Identificador autonumérico del Comentario
      /// </summary>
      public Guid? ComentarioID{
         get{ return this.comentarioID; }
         set{ this.comentarioID = value; }
      }
      private string cuerpo;
      /// <summary>
      /// Cuerpo
      /// </summary>
      public string Cuerpo{
         get{ return this.cuerpo; }
         set{ this.cuerpo = value; }
      }
      private DateTime? fechaComentario;
      /// <summary>
      /// Fecha del Comentario
      /// </summary>
      public DateTime? FechaComentario{
         get{ return this.fechaComentario; }
         set{ this.fechaComentario = value; }
      }
      private bool? estatus;
      /// <summary>
      /// Estatus
      /// </summary>
      public bool? Estatus{
         get{ return this.estatus; }
         set{ this.estatus = value; }
      }
      private UsuarioSocial usuarioSocial;
      /// <summary>
      /// UsuarioSocial
      /// </summary>
      public UsuarioSocial UsuarioSocial{
         get{ return this.usuarioSocial; }
         set{ this.usuarioSocial = value; }
      }
      private Ranking ranking;
      /// <summary>
      /// Rating
      /// </summary>
      public Ranking Ranking
      {
          get { return this.ranking; }
          set { this.ranking = value; }
      }

      public object Clone ( )
      {
          return this.MemberwiseClone( );
      }

      public Guid? GUID
      {
          get
          {
              return this.comentarioID;
          }
          set
          {
              this.comentarioID = value;
          }
      }

      public string TextoNotificacion
      {
          get
          {
              return GP.SocialEngine.Properties.Resources.ComentarioNotificacion;
          }
          set
          {
              throw new NotImplementedException();
          }
      }

      public string URLReferencia
      {
          get
          {
              return "VerPublicacion.aspx";
          }
          set
          {
              throw new NotImplementedException();
          }
      }
      private IAppSocial appSocial;
      /// <summary>
      /// Aplicacion social que se quiere publicar
      /// </summary>
      public IAppSocial AppSocial
      {
          get { return this.appSocial; }
          set { this.appSocial = value; }
      }
      private ETipoPublicacion? tipoPublicacion;
      /// <summary>
      /// ETipoPublicacion
      /// </summary>
      public ETipoPublicacion? TipoPublicacion
      {
          get { return this.tipoPublicacion; }
          set { this.tipoPublicacion = value; }
      }
   } 
}
