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
   /// Guarda un registro de PalabraClave en la BD
   /// </summary>
   internal class PalabraClaveInsHlp 
   { 
      /// <summary>
      /// Crea un registro de PalabraClave en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="palabraClave">PalabraClave que desea crear</param>
      public void Action(IDataContext dctx, PalabraClave palabraClave)
      {
         object myFirm = new object();
         string sError = String.Empty;
         if (palabraClave == null)
            sError += ", palabraClave";
         if (sError.Length > 0)
            throw new Exception("PalabraClaveInsHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
         if (palabraClave.Tag == null)
             sError += ", palabraClave.Tag";
         if (palabraClave.TipoPalabraClave == null)
             sError += ", palabraClave.TipoPalabraClave";
         if (sError.Length > 0)
             throw new Exception("PalabraClaveInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.Busqueda.DAO", 
         "PalabraClaveInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try
         {
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } 
         catch(Exception ex)
         {
      throw new StandardException(MessageType.Error, "", "PalabraClaveInsHlp: No se pudo conectar a la base de datos", "POV.ContenidosDigital.Busqueda.DAO", 
         "PalabraClaveInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO PalabraClave (Tag, TipoPalabraClave) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // palabraClave.Tag
         sCmd.Append(" @dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (palabraClave.Tag == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = palabraClave.Tag;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // palabraClave.TipoPalabraClave
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (palabraClave.TipoPalabraClave == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = palabraClave.TipoPalabraClave;
         sqlParam.DbType = DbType.Byte;
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
            throw new Exception("PalabraClaveInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
         } 
         finally
         {
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PalabraClaveInsHlp: Ocurrio un error al ingresar el registro.");
      }
   } 
}
