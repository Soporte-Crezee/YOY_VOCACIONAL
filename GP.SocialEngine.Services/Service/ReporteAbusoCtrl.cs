using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.DA;
using GP.SocialEngine.DAO;
using GP.SocialEngine.BO;
using POV.CentroEducativo.BO;

namespace GP.SocialEngine.Service { 
   /// <summary>
   /// Controlador del objeto reporte abuso
   /// </summary>
   public class ReporteAbusoCtrl { 
      /// <summary>
      /// Consulta registros de ReporteAbusoRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="reporteAbusoRetHlp">ReporteAbusoRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de ReporteAbusoRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, ReporteAbuso reporteabuso){
         ReporteAbusoRetHlp da = new ReporteAbusoRetHlp();
         DataSet ds = da.Action(dctx, reporteabuso);
         return ds;
      }

       /// <summary>
       /// Consulta los registros de ReportesAbuso y los transforma al formato de Notificaciones
       /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="reporteAbuso">ReporteAbuso que provee el criterio de selección para realizar la consulta</param>
      /// <para name="usuarioSocila">Usuario que consulta las notificaciones por reporte abuso</para>
      /// <returns>El DataSet que contiene la información de los reportes abuso</returns>
      public DataSet RetriveReportesAbusoINotificable(IDataContext dctx,ReporteAbuso reporteAbuso,UsuarioSocial usuarioSocial, List<AreaConocimiento> areasConocimiento, long universidadID)
      {
         
         //Consultar el usuario que desea ver los reportes de abuso y sus grupos sociales
          SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
          DataSet dsHub =socialHubCtrl.RetrieveSocialHubUsuario(dctx, new SocialHub { SocialProfileType = ESocialProfileType.USUARIOSOCIAL, SocialProfile = new UsuarioSocial {UsuarioSocialID = usuarioSocial.UsuarioSocialID} });

          if (dsHub.Tables["SocialHub"].Rows.Count <= 0) //El docente no tiene socialHub
              return null;

          SocialHub socialHub = socialHubCtrl.LastDataRowToSocialHub(dsHub);
          List<GrupoSocial> lsGrupoSocial = (new GrupoSocialCtrl()).RetrieveGruposSocialSocialHub(dctx, socialHub);

          DataSet ds = new DataSet();
          ds.Tables.Add(new DataTable("Notificacion"));
          ds.Tables["Notificacion"].Columns.Add(new DataColumn("NotificacionID", typeof(Guid)));
          ds.Tables["Notificacion"].Columns.Add(new DataColumn("FechaRegistro", typeof(DateTime)));
          ds.Tables["Notificacion"].Columns.Add(new DataColumn("EmisorID", typeof(long)));
          ds.Tables["Notificacion"].Columns.Add(new DataColumn("ReceptorID", typeof(long)));
          ds.Tables["Notificacion"].Columns.Add(new DataColumn("NotificableID", typeof(Guid)));
          ds.Tables["Notificacion"].Columns.Add(new DataColumn("TipoNotificacion", typeof(short)));
          ds.Tables["Notificacion"].Columns.Add(new DataColumn("EstatusNotificacion", typeof(short)));

          UsuarioGrupoCtrl usuarioGrupoCtrl = new UsuarioGrupoCtrl();
          foreach (GrupoSocial grupoSocial in lsGrupoSocial)
           {
             DataSet dsUsurGrupo= usuarioGrupoCtrl.Retrieve(dctx, new UsuarioGrupo {UsuarioSocial = usuarioSocial,Estatus = true}, grupoSocial, areasConocimiento, universidadID);
             if (dsUsurGrupo.Tables["UsuarioGrupo"].Rows.Count != 1)
                 continue;
               UsuarioGrupo usuarioGrupo = usuarioGrupoCtrl.LastDataRowToUsuarioGrupo(dsUsurGrupo);
               if ((bool) (!usuarioGrupo.EsModerador))
                   return null;

              //Consultar reportes de abuso del grupo
               DataSet dsReporteAbuso = Retrieve(dctx, new ReporteAbuso { EstadoReporteAbuso = reporteAbuso.EstadoReporteAbuso, GrupoSocial = grupoSocial });

               foreach (DataRow rp in dsReporteAbuso.Tables["ReporteAbuso"].Rows)
               {
                   reporteAbuso = DataRowToReporteAbuso(rp);
                   AddDataRowToINotificable(ds,reporteAbuso,usuarioSocial);
               }
           }

           return ds;
      }

       /// <summary>
       /// Consulta los reportes de abuso de un docente
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="reporteAbuso">provee criterios de selección para realizar la consulta</param>
       /// <param name="usuarioSocial">Usuario que consulta los reportes abuso</param>
       /// <returns>Lista de Reportes de Abuso</returns>
      public List<ReporteAbuso> RetrieveReportesAbusoDocente(IDataContext dctx,ReporteAbuso reporteAbuso,UsuarioSocial usuarioSocial, List<AreaConocimiento> areasConocimiento, long universidadID)
       {
           List<ReporteAbuso> ls = new List<ReporteAbuso>();

           //Consultar el usuario que desea ver los reportes de abuso y sus grupos sociales
           SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
           DataSet dsHub = socialHubCtrl.RetrieveSocialHubUsuario(dctx, new SocialHub { SocialProfileType = ESocialProfileType.USUARIOSOCIAL, SocialProfile = new UsuarioSocial { UsuarioSocialID = usuarioSocial.UsuarioSocialID } });

           if (dsHub.Tables["SocialHub"].Rows.Count <= 0) //El docente no tiene socialHub
               return null;

           UsuarioGrupoCtrl usuarioGrupoCtrl = new UsuarioGrupoCtrl();
           SocialHub socialHub = socialHubCtrl.LastDataRowToSocialHub(dsHub);
           List<GrupoSocial> lsGrupoSocial = (new GrupoSocialCtrl()).RetrieveGruposSocialSocialHub(dctx, socialHub);
           foreach (GrupoSocial grupoSocial in lsGrupoSocial)
           {
               DataSet dsUsurGrupo = usuarioGrupoCtrl.Retrieve(dctx, new UsuarioGrupo { UsuarioSocial = usuarioSocial, Estatus = true }, grupoSocial, areasConocimiento, universidadID);
               if (dsUsurGrupo.Tables["UsuarioGrupo"].Rows.Count != 1)
                   continue;
               UsuarioGrupo usuarioGrupo = usuarioGrupoCtrl.LastDataRowToUsuarioGrupo(dsUsurGrupo);

               if ((bool)(!usuarioGrupo.EsModerador))
                   return null;

               DataSet dsReporteAbuso = Retrieve(dctx, new ReporteAbuso { EstadoReporteAbuso = reporteAbuso.EstadoReporteAbuso, GrupoSocial = grupoSocial });
               foreach (DataRow rp in dsReporteAbuso.Tables["ReporteAbuso"].Rows)
               {
                   reporteAbuso = DataRowToReporteAbuso(rp);
                   ls.Add(reporteAbuso);
               }
           }
           //Consultar reportes de abuso del grupo
           return ls;
       }

       /// <summary>
       /// Consulta una pagina de los reportes de abuso de un docente
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="size">Tamaño de la pagina a consultar</param>
       /// <param name="page">Pagina que desea consultar</param>
       /// <param name="parametros">Provee criterios de selección para realizar la consulta</param>
       /// <param name="tipo">Lista de ETipoContenido</param>
       /// <param name="estados">Lista de EEstadoReporteAbuso</param>
       /// <param name="usuarioSocial">Usuario Docente que consulta los reportes abuso</param>  
       /// <returns>DataSet que contiene el resultado de la consulta</returns>
       public DataSet RetrievePageReportesAbusoDocente(IDataContext dctx,int size,int page,Dictionary<string,string> parametros,List<ETipoContenido> tipo,List<EEstadoReporteAbuso> estados,UsuarioSocial usuarioSocial, List<AreaConocimiento> areasConocimiento, long universidadID)
      {
          //Consultar el usuario que desea ver los reportes de abuso y sus grupos sociales
          SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
          DataSet dsHub = socialHubCtrl.RetrieveSocialHubUsuario(dctx, new SocialHub { SocialProfileType = ESocialProfileType.USUARIOSOCIAL, SocialProfile = new UsuarioSocial { UsuarioSocialID = usuarioSocial.UsuarioSocialID } });

          if (dsHub.Tables["SocialHub"].Rows.Count <= 0) //El docente no tiene socialHub
              return null;

          UsuarioGrupoCtrl usuarioGrupoCtrl = new UsuarioGrupoCtrl();
          SocialHub socialHub = socialHubCtrl.LastDataRowToSocialHub(dsHub);
          List<GrupoSocial> lsGrupoSocial = (new GrupoSocialCtrl()).RetrieveGruposSocialSocialHub(dctx, socialHub);
         
          DataSet ds = null;
          //constantes 
          const string sort = "FechaReporte";
           const string sortorder = "desc";
          List<GrupoSocial> lsGruposActivos = new List<GrupoSocial>();
          foreach (GrupoSocial grupoSocial in lsGrupoSocial)
          {
              DataSet dsUsurGrupo = usuarioGrupoCtrl.Retrieve(dctx, new UsuarioGrupo { UsuarioSocial = usuarioSocial, Estatus = true }, grupoSocial, areasConocimiento, universidadID);
              if (dsUsurGrupo.Tables["UsuarioGrupo"].Rows.Count != 1)
                  continue;
              UsuarioGrupo usuarioGrupo = usuarioGrupoCtrl.LastDataRowToUsuarioGrupo(dsUsurGrupo);

              if ((bool)(!usuarioGrupo.EsModerador))
                  continue;
              lsGruposActivos.Add(grupoSocial);
          }
          if(lsGruposActivos.Count>0)
              ds = (new ReporteAbusoDARetHlp()).Action(dctx, size, page,sort,sortorder, parametros,tipo,estados,lsGruposActivos);

           return ds;
      }
      
      public ReporteAbuso RetrieveComplete(IDataContext dctx,ReporteAbuso reporteAbuso)
       {
           if(reporteAbuso==null || reporteAbuso.ReporteAbusoID==null)
               throw new Exception("RetrieveComplete:ReporteAbuso no puede ser nulo");
           DataSet ds = Retrieve(dctx, reporteAbuso);
           if (ds.Tables["ReporteAbuso"].Rows.Count != 1)
               return null;

           ReporteAbuso reporte = LastDataRowToReporteAbuso(ds);
           UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
           switch (reporte.TipoContenido)
           {
               case ETipoContenido.PUBLICACION:
                   {
                      PublicacionCtrl publicacionCtrl = new PublicacionCtrl();
                      reporte.Reportable = publicacionCtrl.LastDataRowToPublicacion(publicacionCtrl.Retrieve(dctx, new Publicacion { PublicacionID = reporte.Reportable.GUID }));
                   }
                   break;
               case ETipoContenido.COMENTARIO:
                   {
                       ComentarioCtrl comentarioCtrl = new ComentarioCtrl();
                       PublicacionCtrl publicacionCtrl = new PublicacionCtrl();
                       Publicacion padre=publicacionCtrl.RetrievePublicacionPadre(dctx, new Comentario { ComentarioID = reporte.Reportable.GUID });
                       reporte.Reportable = comentarioCtrl.LastDataRowToComentario(comentarioCtrl.Retrieve(dctx, new Comentario { ComentarioID = reporte.Reportable.GUID },padre));
                   }
                   break;
           }
           //Consultar Usuario Reportarte
           reporte.Reportante = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = reporte.Reportante.UsuarioSocialID }));
           //Consultar Usuario Reportado
           reporte.Reportado = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = reporte.Reportado.UsuarioSocialID }));

           return reporte;
       }
      /// <summary>
      /// Crea un registro de ReporteAbusoInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="reporteAbusoInsHlp">ReporteAbusoInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, ReporteAbuso reporteabuso){
         ReporteAbusoInsHlp da = new ReporteAbusoInsHlp();
         da.Action(dctx, reporteabuso);
      }
   
      /// <summary>
      /// Actualiza de manera optimista un registro de ReporteAbusoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="reporteAbusoUpdHlp">ReporteAbusoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">ReporteAbusoUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, ReporteAbuso reporteabuso, ReporteAbuso previous){
         ReporteAbusoUpdHlp da = new ReporteAbusoUpdHlp();
         da.Action(dctx, reporteabuso, previous);
      }
      /// <summary>
      /// Crea un objeto de ReporteAbuso a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de ReporteAbuso</param>
      /// <returns>Un objeto de ReporteAbuso creado a partir de los datos</returns>
      public ReporteAbuso LastDataRowToReporteAbuso(DataSet ds) {
         if (!ds.Tables.Contains("ReporteAbuso"))
            throw new Exception("LastDataRowToReporteAbuso: DataSet no tiene la tabla ReporteAbuso");
         int index = ds.Tables["ReporteAbuso"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToReporteAbuso: El DataSet no tiene filas");
         return this.DataRowToReporteAbuso(ds.Tables["ReporteAbuso"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de ReporteAbuso a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de ReporteAbuso</param>
      /// <returns>Un objeto de ReporteAbuso creado a partir de los datos</returns>
      public ReporteAbuso DataRowToReporteAbuso(DataRow row){
         ReporteAbuso reporteAbuso = new ReporteAbuso();
         reporteAbuso.Reportado = new UsuarioSocial();
         reporteAbuso.Reportante = new UsuarioSocial();
         reporteAbuso.GrupoSocial = new GrupoSocial();
         if (row.IsNull("ReporteAbusoID"))
            reporteAbuso.ReporteAbusoID = null;
         else
            reporteAbuso.ReporteAbusoID = (Guid)Convert.ChangeType(row["ReporteAbusoID"], typeof(Guid));
         if (row.IsNull("FechaReporte"))
            reporteAbuso.FechaReporte = null;
         else
            reporteAbuso.FechaReporte = (DateTime)Convert.ChangeType(row["FechaReporte"], typeof(DateTime));
         if (row.IsNull("FechaFinReporte"))
            reporteAbuso.FechaFinReporte = null;
         else
            reporteAbuso.FechaFinReporte = (DateTime)Convert.ChangeType(row["FechaFinReporte"], typeof(DateTime));
         if (row.IsNull("TipoContenido"))
             reporteAbuso.TipoContenido = null;
         else
         {
             reporteAbuso.TipoContenido= (ETipoContenido?)(short)Convert.ChangeType(row["TipoContenido"], typeof(short));

             if (row.IsNull("ReportableID"))
                 reporteAbuso.Reportable = null;
             else
             {
                 switch (reporteAbuso.TipoContenido)
                 {
                     case ETipoContenido.PUBLICACION :
                         reporteAbuso.Reportable = new Publicacion { PublicacionID = (Guid)Convert.ChangeType(row["ReportableID"], typeof(Guid)) };

                         break;
                     case ETipoContenido.COMENTARIO:
                         reporteAbuso.Reportable = new Comentario { ComentarioID = (Guid)Convert.ChangeType(row["ReportableID"], typeof(Guid)) };
                         break;
                 }
             }
         }
             

          if (row.IsNull("EstatusReporte"))
            reporteAbuso.EstadoReporteAbuso = null;
         else
            reporteAbuso.EstadoReporteAbuso = (EEstadoReporteAbuso?)(short)Convert.ChangeType(row["EstatusReporte"], typeof(short));
         if (row.IsNull("ReportadoID"))
            reporteAbuso.Reportado.UsuarioSocialID = null;
         else
            reporteAbuso.Reportado.UsuarioSocialID = (long)Convert.ChangeType(row["ReportadoID"], typeof(long));
         if (row.IsNull("ReportanteID"))
            reporteAbuso.Reportante.UsuarioSocialID = null;
         else
            reporteAbuso.Reportante.UsuarioSocialID = (long)Convert.ChangeType(row["ReportanteID"], typeof(long));
         if (row.IsNull("GrupoSocialID"))
            reporteAbuso.GrupoSocial.GrupoSocialID = null;
         else
            reporteAbuso.GrupoSocial.GrupoSocialID = (long)Convert.ChangeType(row["GrupoSocialID"], typeof(long));

         return reporteAbuso;
      }

      private void AddDataRowToINotificable(DataSet ds,ReporteAbuso reporteAbuso,UsuarioSocial usuarioSocial)
      {
          if(!ds.Tables.Contains("Notificacion"))
              return;

          if(!ds.Tables["Notificacion"].Columns.Contains("NotificacionID"))
              return;
          if(!ds.Tables["Notificacion"].Columns.Contains("FechaRegistro"))
              return;
          if(!ds.Tables["Notificacion"].Columns.Contains("EmisorID"))
              return;
          if(!ds.Tables["Notificacion"].Columns.Contains("ReceptorID"))
              return;
          if(!ds.Tables["Notificacion"].Columns.Contains("NotificableID"))
              return;
          if(!ds.Tables["Notificacion"].Columns.Contains("TipoNotificacion"))
              return;
          if (!ds.Tables["Notificacion"].Columns.Contains("EstatusNotificacion"))
              return;

          DataRow dr = ds.Tables["Notificacion"].NewRow();
          dr.SetField("NotificacionID", reporteAbuso.ReporteAbusoID);
          dr.SetField("FechaRegistro",reporteAbuso.FechaReporte);
          dr.SetField("EmisorID",reporteAbuso.Reportado.UsuarioSocialID);
          dr.SetField("ReceptorID",usuarioSocial.UsuarioSocialID);
          dr.SetField("NotificableID",reporteAbuso.Reportable.GUID);
          dr.SetField("TipoNotificacion",ETipoNotificacion.REPORTE_ABUSO);

          switch (reporteAbuso.EstadoReporteAbuso)
          {
                  case EEstadoReporteAbuso.PENDIENTE:
                  dr.SetField("EstatusNotificacion",EEstatusNotificacion.NUEVO);
                  break;
                  case EEstadoReporteAbuso.CONFIRMADO:
                  dr.SetField("EstatusNotificacion",EEstatusNotificacion.CONFIRMADO);
                  break;
                  case EEstadoReporteAbuso.CANCELADO:
                  dr.SetField("EstatusNotificacion",EEstatusNotificacion.ELIMINADO);
                  break;
          }
          ds.Tables["Notificacion"].Rows.Add(dr);
      }

      public DataSet RetrieveLastReportesAbuso(IDataContext dctx,ReporteAbuso reporte,ConfiguracionReporteAbuso configuracion)
      {
          if (reporte == null || reporte.Reportante == null)
              return null;

          ReporteAbusoRetHlp da = new ReporteAbusoRetHlp();
          DataSet ds = da.Action(dctx, reporte, configuracion);

          return ds;
      }
   } 
}
