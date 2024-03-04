using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO { 
   /// <summary>
   /// Guarda un registro de PruebaDinamica en la BD
   /// </summary>
   internal class PruebaDinamicaInsHlp { 
      /// <summary>
      /// Crea un registro de PruebaDinamica en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="pruebaDinamica">PruebaDinamica que desea crear</param>
      public void Action(IDataContext dctx, PruebaDinamica pruebaDinamica){
         object myFirm = new object();
         string sError = String.Empty;
         if (pruebaDinamica == null)
            sError += ", PruebaDinamica";
         if (sError.Length > 0)
            throw new Exception("PruebaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (pruebaDinamica.PruebaID == null)
            sError += ", PruebaID";
         if (sError.Length > 0)
            throw new Exception("PruebaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "PruebaDinamicaInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PruebaDinamicaInsHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "PruebaDinamicaInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO PruebaDinamica (PruebaID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // pruebaDinamica.PruebaID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (pruebaDinamica.PruebaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pruebaDinamica.PruebaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PruebaDinamicaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PruebaDinamicaInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
