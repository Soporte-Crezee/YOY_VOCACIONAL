using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;
using POV.ContenidosDigital.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Elimina un registro de ContenidoDigitalCurso en la BD
   /// </summary>
   internal class ContenidoDigitalCursoDelHlp { 
      /// <summary>
      /// Elimina un registro de ContenidoDigitalCursoDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="contenidoDigitalCursoDelHlp">ContenidoDigitalCursoDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenidoDigital, ContenidoDigital contenidoDigital){
         object myFirm = new object();
         string sError = String.Empty;
         if (aAgrupadorContenidoDigital == null)
            sError += ", AgrupadorSimple";
         if (sError.Length > 0)
            throw new Exception("ContenidoDigitalCursoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (aAgrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
            sError += ", AgrupadorContenidoDigitalID";
         if (sError.Length > 0)
            throw new Exception("ContenidoDigitalCursoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (contenidoDigital == null)
            sError += ", ContenidoDigital";
         if (sError.Length > 0)
            throw new Exception("ContenidoDigitalCursoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (contenidoDigital.ContenidoDigitalID == null)
            sError += ", ContenidoDigitalID";
         if (sError.Length > 0)
            throw new Exception("ContenidoDigitalCursoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "ContenidoDigitalCursoDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ContenidoDigitalCursoDelHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO", 
         "ContenidoDigitalCursoDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM CursoDetalle ");
         if (aAgrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
            sCmd.Append(" WHERE AgrupadorContenidoDigitalID IS NULL ");
         else{ 
            // agrupadorSimple.AgrupadorContenidoDigitalID
            sCmd.Append(" WHERE AgrupadorContenidoDigitalID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = aAgrupadorContenidoDigital.AgrupadorContenidoDigitalID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (contenidoDigital.ContenidoDigitalID == null)
            sCmd.Append(" AND ContenidoDigitalID IS NULL ");
         else{ 
            // contenidoDigital.ContenidoDigitalID
            sCmd.Append(" AND ContenidoDigitalID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = contenidoDigital.ContenidoDigitalID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ContenidoDigitalCursoDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ContenidoDigitalCursoDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
