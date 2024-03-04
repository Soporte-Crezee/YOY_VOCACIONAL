using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.Busqueda.BO;
using POV.Profesionalizacion.BO;

namespace POV.ContenidosDigital.Busqueda.DAO 
{ 
   /// <summary>
   /// Guarda un registro de PalabraClaveContenidoDigital en la BD
   /// </summary>
   internal class PalabraClaveContenidoDigitalInsHlp 
   { 
      /// <summary>
      /// Crea un registro de palabraClaveContenidoDigital, contenidoDigitalAgrupador en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="palabraClaveContenidoDigital">PalabraClaveContenidoDigital</param>
      /// <param name="contenidoDigitalAgrupador">ContenidoDigitalAgrupador</param>
      public void Action(IDataContext dctx, PalabraClaveContenidoDigital palabraClaveContenidoDigital, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
      {
         object myFirm = new object();
         string sError = String.Empty;
         if (palabraClaveContenidoDigital == null)
            sError += ", palabraClaveContenidoDigital";
         if (contenidoDigitalAgrupador == null)
            sError += ", contenidoDigitalAgrupador";
         if (sError.Length > 0)
            throw new Exception("PalabraClaveContenidoDigitalInsHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
         if (palabraClaveContenidoDigital.PalabraClaveContenidoID == null)
            sError += ", palabraClaveContenidoDigital.PalabraClaveContenidoID";
         if (contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID == null)
            sError += ", contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID";
         if (sError.Length > 0)
            throw new Exception("PalabraClaveContenidoDigitalInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.Busqueda.DAO", 
         "PalabraClaveContenidoDigitalInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try
         {
             dctx.OpenConnection(myFirm);
             sqlCmd = dctx.CreateCommand();
         }
         catch (Exception ex)
         {
             throw new StandardException(MessageType.Error, "",
                                         "PalabraClaveContenidoDigitalInsHlp: No se pudo conectar a la base de datos",
                                         "POV.ContenidosDigital.Busqueda.DAO",
                                         "PalabraClaveContenidoDigitalInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO PalabraClaveContenidoDigital (PalabraClaveContenidoID, ContenidoDigitalAgrupadorID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // palabraClaveContenidoDigital.PalabraClaveContenidoID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (palabraClaveContenidoDigital.PalabraClaveContenidoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = palabraClaveContenidoDigital.PalabraClaveContenidoID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID;
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
            throw new Exception("PalabraClaveContenidoDigitalInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
         } 
         finally
         {
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PalabraClaveContenidoDigitalInsHlp: Ocurrio un error al ingresar el registro.");
      }
   } 
}
