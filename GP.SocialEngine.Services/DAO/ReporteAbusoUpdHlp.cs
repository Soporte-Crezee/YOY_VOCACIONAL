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
   /// Actualiza un reporte abuso en la base de datos
   /// </summary>
   public class ReporteAbusoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de ReporteAbuso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="reporteAbuso">ReporteAbuso que tiene los datos nuevos</param>
      /// <param name="anterior">ReporteAbuso que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, ReporteAbuso reporteabuso,ReporteAbuso anterior){
         object myFirm = new object();
         string sError = String.Empty;
         if (reporteabuso == null)
            sError += ", ReporteAbuso";
         if (anterior == null)
            sError += ", anterior";
         if (sError.Length > 0)
            throw new Exception("ReporteAbusoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (reporteabuso.ReporteAbusoID == null)
            sError += ", ReporteAbusoID";
         if (reporteabuso.FechaReporte == null)
            sError += ", FechaReporte";
         if (reporteabuso.TipoContenido == null)
            sError += ", TipoContenido";
         if (reporteabuso.EstadoReporteAbuso == null)
            sError += ", EstadoReporteAbuso";
         if (reporteabuso.Reportado == null)
            sError += ", Reportado";
         if (reporteabuso.Reportante == null)
            sError += ", Reportante";
         if (reporteabuso.GrupoSocial == null)
            sError += ", GrupoSocial";
         if (reporteabuso.Reportable == null)
            sError += ", Reportable";
         if (reporteabuso.FechaFinReporte == null)
            sError += ", FechaFinReporte";
         if (anterior.ReporteAbusoID == null)
            sError += ", ReporteAbusoID";
         if (anterior.FechaReporte == null)
            sError += ", FechaReporte";
         if (anterior.TipoContenido == null)
            sError += ", TipoContenido";
         if (anterior.EstadoReporteAbuso == null)
            sError += ", EstadoReporteAbuso";
         if (anterior.Reportado == null)
            sError += ", Reportado";
         if (anterior.Reportante == null)
            sError += ", Reportante";
         if (anterior.GrupoSocial == null)
            sError += ", GrupoSocial";
         if (anterior.Reportable == null)
            sError += ", Reportable";
         if (sError.Length > 0)
            throw new Exception("ReporteAbusoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (reporteabuso.Reportable.GUID == null)
            sError += ", ReportableID";
         if (reporteabuso.Reportado.UsuarioSocialID == null)
            sError += ", ReportadoID";
         if (reporteabuso.Reportante.UsuarioSocialID == null)
            sError += ", ReportanteID";
         if (reporteabuso.GrupoSocial.GrupoSocialID == null)
            sError += ", GrupoSocialID";
         if (anterior.Reportable.GUID == null)
            sError += ", ReportableID";
         if (anterior.Reportado.UsuarioSocialID == null)
            sError += ", ReportadoID";
         if (anterior.Reportante.UsuarioSocialID == null)
            sError += ", ReportanteID";
         if (anterior.GrupoSocial.GrupoSocialID == null)
            sError += ", GrupoSocialID";
         if (sError.Length > 0)
            throw new Exception("ReporteAbusoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (reporteabuso.ReporteAbusoID != anterior.ReporteAbusoID) {
         sError = "Los parametros no coinciden";
      }
         if (sError.Length > 0)
            throw new Exception("ReporteAbusoUpdHlp: " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "ReporteAbusoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ReporteAbusoUpdHlp: Ocurrió un error al conectarse a la base de datos", "GP.SocialEngine.DAO", 
         "ReporteAbusoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE REPORTEABUSO ");
         // reporteabuso.FechaFinReporte
         sCmd.Append(" SET FECHAFINREPORTE =@dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (reporteabuso.FechaFinReporte == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = reporteabuso.FechaFinReporte;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // reporteabuso.EstadoReporteAbuso
         sCmd.Append(" ,ESTATUSREPORTE =@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (reporteabuso.EstadoReporteAbuso == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = reporteabuso.EstadoReporteAbuso;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.ReporteAbusoID
         sCmd.Append(" WHERE REPORTEABUSOID =@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (anterior.ReporteAbusoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.ReporteAbusoID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.FechaReporte
         sCmd.Append(" AND FECHAREPORTE =@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (anterior.FechaReporte == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.FechaReporte;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.TipoContenido
         sCmd.Append(" AND TIPOCONTENIDO =@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (anterior.TipoContenido == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.TipoContenido;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.EstadoReporteAbuso
         sCmd.Append(" AND ESTATUSREPORTE =@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (anterior.EstadoReporteAbuso == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.EstadoReporteAbuso;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.Reportado.UsuarioSocialID
         sCmd.Append(" AND REPORTADOID =@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (anterior.Reportado.UsuarioSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.Reportado.UsuarioSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.Reportante.UsuarioSocialID
         sCmd.Append(" AND REPORTANTEID =@dbp4ram8 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram8";
         if (anterior.Reportante.UsuarioSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.Reportante.UsuarioSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.GrupoSocial.GrupoSocialID
         sCmd.Append(" AND GRUPOSOCIALID =@dbp4ram9 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram9";
         if (anterior.GrupoSocial.GrupoSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.GrupoSocial.GrupoSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ReporteAbusoUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ReporteAbusoUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado.");
      }
   } 
}
