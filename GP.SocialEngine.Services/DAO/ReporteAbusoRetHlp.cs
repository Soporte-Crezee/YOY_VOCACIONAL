// DAO de sistema, para implementacion
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Consultar los reportes de abuso de base de datos
   /// </summary>
   public class ReporteAbusoRetHlp { 
      /// <summary>
      /// Consulta registros de ReporteAbuso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="reporteAbuso">ReporteAbuso que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de ReporteAbuso generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, ReporteAbuso reporteabuso){
         object myFirm = new object();
         string sError = "";
         if (reporteabuso == null)
            sError += ", ReporteAbuso";
         if (sError.Length > 0)
            throw new Exception("ReporteAbusoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
      if (reporteabuso.GrupoSocial == null) {
         reporteabuso.GrupoSocial = new GrupoSocial();
      }
      if (reporteabuso.Reportado == null) {
         reporteabuso.Reportado = new UsuarioSocial();
      }
      if (reporteabuso.Reportante == null) {
         reporteabuso.Reportante = new UsuarioSocial();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "ReporteAbusoRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ReporteAbusoRetHlp: Hubo un error al conectarse a la base de datos", "GP.SocialEngine.DAO", 
         "ReporteAbusoRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT REPORTEABUSOID,FECHAREPORTE,FECHAFINREPORTE,REPORTABLEID,TIPOCONTENIDO,ESTATUSREPORTE,REPORTADOID,REPORTANTEID,GRUPOSOCIALID ");
         sCmd.Append(" FROM REPORTEABUSO ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (reporteabuso.ReporteAbusoID != null){
            s_VarWHERE.Append(" REPORTEABUSOID =@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = reporteabuso.ReporteAbusoID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (reporteabuso.FechaReporte != null){
            s_VarWHERE.Append(" AND FECHAREPORTE =@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = reporteabuso.FechaReporte;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (reporteabuso.FechaFinReporte != null){
            s_VarWHERE.Append(" AND FECHAFINREPORTE =@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = reporteabuso.FechaFinReporte;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (reporteabuso.TipoContenido != null){
            s_VarWHERE.Append(" AND TIPOCONTENIDO =@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = reporteabuso.TipoContenido;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (reporteabuso.EstadoReporteAbuso != null){
            s_VarWHERE.Append(" AND ESTATUSREPORTE =@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = reporteabuso.EstadoReporteAbuso;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (reporteabuso.Reportado.UsuarioSocialID != null){
            s_VarWHERE.Append(" AND REPORTADOID =@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = reporteabuso.Reportado.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (reporteabuso.Reportante.UsuarioSocialID != null){
            s_VarWHERE.Append(" AND REPORTANTEID =@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = reporteabuso.Reportante.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (reporteabuso.GrupoSocial.GrupoSocialID != null){
            s_VarWHERE.Append(" AND GRUPOSOCIALID =@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            sqlParam.Value = reporteabuso.GrupoSocial.GrupoSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (reporteabuso.Reportable != null && reporteabuso.Reportable.GUID != null)
         {
             s_VarWHERE.Append(" AND REPORTABLEID =@dbp4ram9 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram9";
             sqlParam.Value = reporteabuso.Reportable.GUID;
             sqlParam.DbType = DbType.Guid;
             sqlCmd.Parameters.Add(sqlParam);
         }
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
            if (s_VarWHEREres.StartsWith("AND "))
               s_VarWHEREres = s_VarWHEREres.Substring(4);
            else if (s_VarWHEREres.StartsWith("OR "))
               s_VarWHEREres = s_VarWHEREres.Substring(3);
            else if (s_VarWHEREres.StartsWith(","))
               s_VarWHEREres = s_VarWHEREres.Substring(1);
            sCmd.Append(" WHERE " + s_VarWHEREres);
         }
         sCmd.Append(" ORDER BY FECHAREPORTE ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "ReporteAbuso");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ReporteAbusoRetHlp: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }

      /// <summary>
      /// Consulta registros de ReporteAbuso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="reporteAbuso">ReporteAbuso que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de ReporteAbuso generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, ReporteAbuso reporteabuso,ConfiguracionReporteAbuso configuracion)
      {
          object myFirm = new object();
          string sError = "";
          if (reporteabuso == null)
              sError += ", ReporteAbuso";

          if (configuracion == null)
              sError += ", Configuracion";

          if (sError.Length > 0)
              throw new Exception("ReporteAbusoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));

          if(reporteabuso.Reportante==null)
              sError += ", Reportante";

          if (sError.Length > 0)
              throw new Exception("ReporteAbusoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));

          if (reporteabuso.Reportante.UsuarioSocialID == null)
              sError += " ,ReportanteID";

          if (sError.Length > 0)
              throw new Exception("ReporteAbusoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));


          if (reporteabuso.GrupoSocial == null)
          {
              reporteabuso.GrupoSocial = new GrupoSocial();
          }
          if (reporteabuso.Reportado == null)
          {
              reporteabuso.Reportado = new UsuarioSocial();
          }
          
          if (dctx == null)
              throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                 "ReporteAbusoRetHlp", "Action", null, null);
          DbCommand sqlCmd = null;
          try
          {
              dctx.OpenConnection(myFirm);
              sqlCmd = dctx.CreateCommand();
          }
          catch (Exception ex)
          {
              throw new StandardException(MessageType.Error, "", "ReporteAbusoRetHlp: Hubo un error al conectarse a la base de datos", "GP.SocialEngine.DAO",
                 "ReporteAbusoRetHlp", "Action", null, null);
          }
          DbParameter sqlParam;
          StringBuilder sCmd = new StringBuilder();
          sCmd.Append(" SELECT REPORTEABUSOID,FECHAREPORTE,FECHAFINREPORTE,REPORTABLEID,TIPOCONTENIDO,ESTATUSREPORTE,REPORTADOID,REPORTANTEID,GRUPOSOCIALID ");
          sCmd.Append(" FROM REPORTEABUSO ");
          StringBuilder s_VarWHERE = new StringBuilder();
          
       
        
          if (reporteabuso.EstadoReporteAbuso != null)
          {
              s_VarWHERE.Append(" AND ESTATUSREPORTE =@dbp4ram5 ");
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "dbp4ram5";
              sqlParam.Value = reporteabuso.EstadoReporteAbuso;
              sqlParam.DbType = DbType.Int16;
              sqlCmd.Parameters.Add(sqlParam);
          }
          if (reporteabuso.Reportante.UsuarioSocialID != null)
          {
              s_VarWHERE.Append(" AND REPORTANTEID =@dbp4ram6 ");
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "dbp4ram6";
              sqlParam.Value = reporteabuso.Reportante.UsuarioSocialID;
              sqlParam.DbType = DbType.Int64;
              sqlCmd.Parameters.Add(sqlParam);
          }
         
          if (reporteabuso.GrupoSocial.GrupoSocialID != null)
          {
              s_VarWHERE.Append(" AND GRUPOSOCIALID =@dbp4ram7 ");
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "dbp4ram7";
              sqlParam.Value = reporteabuso.GrupoSocial.GrupoSocialID;
              sqlParam.DbType = DbType.Int64;
              sqlCmd.Parameters.Add(sqlParam);
          }

           if (configuracion.FechaConsulta != null)
          {
              s_VarWHERE.Append(" AND (FECHAREPORTE >= @dbp4ram8 AND FECHAREPORTE <= @dbp4ram9) ");
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "dbp4ram8";
              sqlParam.Value = configuracion.FechaConsulta;
              sqlParam.DbType = DbType.DateTime;
              sqlCmd.Parameters.Add(sqlParam);

              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "dbp4ram9";
              sqlParam.Value = DateTime.Now;
              sqlParam.DbType = DbType.DateTime;
              sqlCmd.Parameters.Add(sqlParam);
          }
         
          string s_VarWHEREres = s_VarWHERE.ToString().Trim();
          if (s_VarWHEREres.Length > 0)
          {
              if (s_VarWHEREres.StartsWith("AND "))
                  s_VarWHEREres = s_VarWHEREres.Substring(4);
              else if (s_VarWHEREres.StartsWith("OR "))
                  s_VarWHEREres = s_VarWHEREres.Substring(3);
              else if (s_VarWHEREres.StartsWith(","))
                  s_VarWHEREres = s_VarWHEREres.Substring(1);
              sCmd.Append(" WHERE " + s_VarWHEREres);
          }
          sCmd.Append(" ORDER BY FECHAREPORTE ASC ");
          DataSet ds = new DataSet();
          DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
          sqlAdapter.SelectCommand = sqlCmd;
          try
          {
              sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
              sqlAdapter.Fill(ds, "ReporteAbuso");
          }
          catch (Exception ex)
          {
              string exmsg = ex.Message;
              try { dctx.CloseConnection(myFirm); }
              catch (Exception) { }
              throw new Exception("ReporteAbusoRetHlp: Hubo un error al consultar los registros. " + exmsg);
          }
          finally
          {
              try { dctx.CloseConnection(myFirm); }
              catch (Exception) { }
          }
          return ds;
      }
   } 
}
