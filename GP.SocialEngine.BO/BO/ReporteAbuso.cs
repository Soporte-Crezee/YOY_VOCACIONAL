using System;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.BO { 
   /// <summary>
   /// Reporte de Abuso Red Social
   /// </summary>
   public class ReporteAbuso:ICloneable,INotificable { 
      private Guid? reporteAbusoID;
      /// <summary>
      /// Identificador único
      /// </summary>
      public Guid? ReporteAbusoID{
         get{ return this.reporteAbusoID; }
         set{ this.reporteAbusoID = value; }
      }
      private DateTime? fechaReporte;
      /// <summary>
      /// FechaReporte
      /// </summary>
      public DateTime? FechaReporte{
         get{ return this.fechaReporte; }
         set{ this.fechaReporte = value; }
      }
      private DateTime? fechaFinReporte;
      /// <summary>
      /// FechaFinReporte
      /// </summary>
      public DateTime? FechaFinReporte{
         get{ return this.fechaFinReporte; }
         set{ this.fechaFinReporte = value; }
      }
      private EEstadoReporteAbuso? estadoReporteAbuso;
      /// <summary>
      /// Estado Reporte
      /// </summary>
      public EEstadoReporteAbuso? EstadoReporteAbuso{
         get{ return this.estadoReporteAbuso; }
         set{ this.estadoReporteAbuso = value; }
      }
      private ETipoContenido? tipoContenido;
      /// <summary>
      /// Tipo de contenido del Reporte
      /// </summary>
      public ETipoContenido? TipoContenido{
         get{ return this.tipoContenido; }
         set{ this.tipoContenido = value; }
      }
      private UsuarioSocial reportado;
      /// <summary>
      /// Usuario Reportado
      /// </summary>
      public UsuarioSocial Reportado{
         get{ return this.reportado; }
         set{ this.reportado = value; }
      }
      private UsuarioSocial reportante;
      /// <summary>
      /// Usuario que reporta
      /// </summary>
      public UsuarioSocial Reportante{
         get{ return this.reportante; }
         set{ this.reportante = value; }
      }
      private GrupoSocial grupoSocial;
      /// <summary>
      /// GrupoSocial del Reporte Abuso
      /// </summary>
      public GrupoSocial GrupoSocial{
         get{ return this.grupoSocial; }
         set{ this.grupoSocial = value; }
      }
      private IReportable reportable;
      /// <summary>
      /// Elemento que se reporta
      /// </summary>
      public IReportable Reportable{
         get{ return this.reportable; }
         set{ this.reportable = value; }
      }

      private string contenido;
      ///<summary>
      ///Contenido del reporte
      ///</summary>
      public string Contenido{
        get{return this.contenido; }
        set{this.contenido=value;}
      }
      public Guid? GUID
      {
          get { return this.ReporteAbusoID; }
          set { this.ReporteAbusoID = value; }
      }

      
      public string TextoNotificacion
      {
          get { return GP.SocialEngine.Properties.Resources.ReporteAbusoNotificacion; }
          set
          {
              throw new NotImplementedException();
          }
      }

      public string URLReferencia
      {
          get
          {
              return "VerReporteAbuso.aspx";
          }
          set
          {
              throw new NotImplementedException();
          }
      }

      public object Clone()
      {
          return this.MemberwiseClone();
      }
   } 
}
