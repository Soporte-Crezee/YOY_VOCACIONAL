using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.Busqueda.BO;

namespace POV.ContenidosDigital.Busqueda.DAO 
{ 
   /// <summary>
   /// Guarda un registro de PalabraClaveContenido en la BD
   /// </summary>
   internal class PalabraClaveContenidoInsHlp 
   { 
      /// <summary>
      /// Crea un registro de PalabraClaveContenido en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="palabraClaveContenido">PalabraClaveContenido que desea crear</param>
      public void Action(IDataContext dctx, APalabraClaveContenido palabraClaveContenido)
      {
         object myFirm = new object();
         string sError = String.Empty;
         if (palabraClaveContenido == null)
            sError += ", palabraClaveContenido";
         if (sError.Length > 0)
            throw new Exception("PalabraClaveContenidoInsHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
         if (palabraClaveContenido.FechaRegistro == null)
            sError += ", palabraClaveContenido.FechaRegistro";
         if (palabraClaveContenido.PalabraClave == null)
            sError += ", palabraClaveContenido.PalabraClave";
         if (palabraClaveContenido.PalabraClave.PalabraClaveID == null)
            sError += ", palabraClaveContenido.PalabraClave.PalabraClaveID";
         if (sError.Length > 0)
            throw new Exception("PalabraClaveContenidoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.Busqueda.DAO", 
         "PalabraClaveContenidoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try
         {
             dctx.OpenConnection(myFirm);
             sqlCmd = dctx.CreateCommand();
         }
         catch (Exception ex)
         {
             throw new StandardException(MessageType.Error, "",
                                         "PalabraClaveContenidoInsHlp: No se pudo conectar a la base de datos",
                                         "POV.ContenidosDigital.Busqueda.DAO",
                                         "PalabraClaveContenidoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO PalabraClaveContenido (FechaRegistro, PalabraClaveID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // palabraClaveContenido.FechaRegistro
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (palabraClaveContenido.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = palabraClaveContenido.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // palabraClaveContenido.PalabraClave.PalabraClaveID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (palabraClaveContenido.PalabraClave.PalabraClaveID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = palabraClaveContenido.PalabraClave.PalabraClaveID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try
         {
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } 
         catch(Exception ex)
         {
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PalabraClaveContenidoInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
         } 
         finally
         {
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PalabraClaveContenidoInsHlp: Ocurrio un error al ingresar el registro.");
      }
   } 
}
