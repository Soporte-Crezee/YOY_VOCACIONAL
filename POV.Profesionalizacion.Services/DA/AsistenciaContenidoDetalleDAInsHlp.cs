using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DA { 
   /// <summary>
   /// Inserta AsistenciaContenidoDigital en la base de datos
   /// </summary>
   public class AsistenciaContenidoDetalleDAInsHlp { 
      /// <summary>
      /// Crea un registro de AsistenciaContenido en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="asistenciaContenido">AsistenciaContenido que desea crear</param>
      public void Action(IDataContext dctx, AAgrupadorContenidoDigital asistencia,long? contenidoDigitalID){
         object myFirm = new object();
         string sError = "";
         if (asistencia == null)
            sError += ", Asistencia";
         if (sError.Length > 0)
            throw new Exception("AsistenciaContenidoDAInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (asistencia.AgrupadorContenidoDigitalID == null)
            sError += ", AgrupadorContenidoDigitalID";
         if (contenidoDigitalID == null)
            sError += ", ContenidoDigitalID";
         if (sError.Length > 0)
            throw new Exception("AsistenciaContenidoDAInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DA", 
         "AsistenciaContenidoDigitalDAInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AsistenciaInsHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DA", 
         "AsistenciaContenidoDigitalDAInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO AsistenciaDetalle (AgrupadorContenidoDigitalID, ContenidoDigitalID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // asistencia.AgrupadorContenidoDigitalID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (asistencia.AgrupadorContenidoDigitalID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = asistencia.AgrupadorContenidoDigitalID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // contenidoDigitalID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (contenidoDigitalID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = contenidoDigitalID;
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
            throw new Exception("AsistenciaContenidoDAInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("AsistenciaContenidoDAInsHlp: Ocurrio un error al ingresar el registro.");
      }
   } 
}
