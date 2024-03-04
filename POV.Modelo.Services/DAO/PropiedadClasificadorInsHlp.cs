using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.BO;
using POV.Modelo.DAO;

namespace POV.Modelo.DAO { 
   /// <summary>
   /// Guarda un registro de PropiedadClasificador en la BD
   /// </summary>
   internal class PropiedadClasificadorInsHlp { 
      /// <summary>
      /// Crea un registro de PropiedadClasificador en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="propiedadClasificador">PropiedadClasificador que desea crear</param>
      public void Action(IDataContext dctx, Clasificador clasificador, PropiedadClasificador propiedadClasifica){
         object myFirm = new object();
         string sError = String.Empty;
         if (clasificador == null)
            sError += ", Clasificador";
         if (propiedadClasifica == null)
            sError += ", PropiedadClasificador";
         if (sError.Length > 0)
            throw new Exception("PropiedadClasificadorInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Modelo.DAO", 
         "PropiedadClasificadorInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PropiedadClasificadorInsHlp: No se pudo conectar a la base de datos", "POV.Modelo.DAO", 
         "PropiedadClasificadorInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO PropiedadClasificador (ClasificadorID, PropiedadID, Descripcion, Activo, FechaRegistro) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // clasificador.ClasificadorID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (clasificador.ClasificadorID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = clasificador.ClasificadorID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // propiedadClasifica.Propiedad.PropiedadID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (propiedadClasifica.Propiedad.PropiedadID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = propiedadClasifica.Propiedad.PropiedadID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // propiedadClasifica.Descripcion
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (propiedadClasifica.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = propiedadClasifica.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // propiedadClasifica.Activo
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (propiedadClasifica.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = propiedadClasifica.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // propiedadClasifica.FechaRegistro
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (propiedadClasifica.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = propiedadClasifica.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PropiedadClasificadorInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PropiedadClasificadorInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
