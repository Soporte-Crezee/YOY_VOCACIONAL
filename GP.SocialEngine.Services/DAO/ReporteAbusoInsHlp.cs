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
   /// Inserta objeto reporte abuso
   /// </summary>
   public class ReporteAbusoInsHlp { 
      /// <summary>
      /// Crea un registro de ReporteAbuso en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="reporteAbuso">ReporteAbuso que desea crear</param>
      public void Action(IDataContext dctx, ReporteAbuso reporteabuso){
         object myFirm = new object();
         string sError = String.Empty;
         if (reporteabuso == null)
            sError += ", ReporteAbuso";
         if (sError.Length > 0)
            throw new Exception("ReporteAbusoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
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
         if (sError.Length > 0)
            throw new Exception("ReporteAbusoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (reporteabuso.Reportable.GUID == null)
            sError += ", ReportableID";
         if (reporteabuso.Reportado.UsuarioSocialID == null)
            sError += ", ReportadoID";
         if (reporteabuso.Reportante.UsuarioSocialID == null)
            sError += ", ReportanteID";
         if (reporteabuso.GrupoSocial.GrupoSocialID == null)
            sError += ", GrupoSocialID";
         if (sError.Length > 0)
            throw new Exception("ReporteAbusoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "ReporteAbusoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ReporteAbusoInsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "ReporteAbusoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO REPORTEABUSO (REPORTEABUSOID,FECHAREPORTE,REPORTABLEID,TIPOCONTENIDO,ESTATUSREPORTE,REPORTADOID,REPORTANTEID,GRUPOSOCIALID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // reporteabuso.ReporteAbusoID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (reporteabuso.ReporteAbusoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = reporteabuso.ReporteAbusoID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         // reporteabuso.FechaReporte
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (reporteabuso.FechaReporte == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = reporteabuso.FechaReporte;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // reporteabuso.Reportable.GUID
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (reporteabuso.Reportable.GUID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = reporteabuso.Reportable.GUID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         // reporteabuso.TipoContenido
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (reporteabuso.TipoContenido == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = reporteabuso.TipoContenido;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         // reporteabuso.EstadoReporteAbuso
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (reporteabuso.EstadoReporteAbuso == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = reporteabuso.EstadoReporteAbuso;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         // reporteabuso.Reportado.UsuarioSocialID
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (reporteabuso.Reportado.UsuarioSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = reporteabuso.Reportado.UsuarioSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // reporteabuso.Reportante.UsuarioSocialID
         sCmd.Append(" ,@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (reporteabuso.Reportante.UsuarioSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = reporteabuso.Reportante.UsuarioSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // reporteabuso.GrupoSocial.GrupoSocialID
         sCmd.Append(" ,@dbp4ram8 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram8";
         if (reporteabuso.GrupoSocial.GrupoSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = reporteabuso.GrupoSocial.GrupoSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ReporteAbusoInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ReporteAbusoInsHlp: Ocurrio un error al ingresar el registro.");
      }
   } 
}
