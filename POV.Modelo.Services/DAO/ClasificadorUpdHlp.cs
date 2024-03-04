using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.BO;

namespace POV.Modelo.DAO { 
   /// <summary>
   /// Actualiza un registro de Clasificador en la BD
   /// </summary>
   internal class ClasificadorUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de ClasificadorUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="clasificadorUpdHlp">ClasificadorUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">ClasificadorUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Clasificador clasificador, Clasificador anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (clasificador == null)
            sError += ", Clasificador";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("ClasificadorUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.ClasificadorID == null)
            sError += ", Anterior ClasificadorID";
         if (sError.Length > 0)
            throw new Exception("ClasificadorUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Modelo.DAO", 
         "ClasificadorUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ClasificadorUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Modelo.DAO", 
         "ClasificadorUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Clasificador ");
         if (clasificador.Nombre == null)
            sCmd.Append(" SET Nombre = NULL ");
         else{ 
            // clasificador.Nombre
            sCmd.Append(" SET Nombre = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = clasificador.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (clasificador.Descripcion == null)
            sCmd.Append(" ,Descripcion = NULL ");
         else{ 
            // clasificador.Descripcion
            sCmd.Append(" ,Descripcion = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = clasificador.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (clasificador.Activo == null)
            sCmd.Append(" ,Activo = NULL ");
         else{ 
            // clasificador.Activo
            sCmd.Append(" ,Activo = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = clasificador.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (clasificador.FechaRegistro == null)
            sCmd.Append(" ,FechaRegistro = NULL ");
         else{ 
            // clasificador.FechaRegistro
            sCmd.Append(" ,FechaRegistro = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = clasificador.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.ClasificadorID == null)
            sCmd.Append(" WHERE ClasificadorID IS NULL ");
         else{ 
            // anterior.ClasificadorID
            sCmd.Append(" WHERE ClasificadorID = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = anterior.ClasificadorID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ClasificadorUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ClasificadorUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
