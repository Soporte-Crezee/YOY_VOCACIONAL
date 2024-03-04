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
   /// Actualiza un registro de PropiedadClasificador en la BD
   /// </summary>
   internal class PropiedadClasificadorUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de PropiedadClasificadorUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="propiedadClasificadorUpdHlp">PropiedadClasificadorUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">PropiedadClasificadorUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx,Clasificador clasificador, PropiedadClasificador propiedadClasifica, PropiedadClasificador anterior){
         object myFirm = new object();
         String sError = string.Empty;
          if (clasificador == null)
              sError += " ,Clasificador";
         if (propiedadClasifica == null)
            sError += ", PropiedadClasificador";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("PropiedadClasificadorUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.Propiedad.PropiedadID == null)
            sError += ", Anterior PropiedadClasificadorID";
          if (clasificador.ClasificadorID == null)
              sError += ", ClasificadorID";
         if (sError.Length > 0)
            throw new Exception("PropiedadClasificadorUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Modelo.DAO", 
         "PropiedadClasificadorUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PropiedadClasificadorUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Modelo.DAO", 
         "PropiedadClasificadorUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE PropiedadClasificador ");
         if (propiedadClasifica.Descripcion == null)
            sCmd.Append(" SET Descripcion = NULL ");
         else{ 
            // clasificador.Descripcion
            sCmd.Append(" SET Descripcion = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = propiedadClasifica.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (propiedadClasifica.Activo == null)
            sCmd.Append(" ,Activo = NULL ");
         else{ 
            // clasificador.Activo
            sCmd.Append(" ,Activo = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = propiedadClasifica.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (propiedadClasifica.FechaRegistro == null)
            sCmd.Append(" ,FechaRegistro = NULL ");
         else{ 
            // clasificador.FechaRegistro
            sCmd.Append(" ,FechaRegistro = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = propiedadClasifica.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Propiedad.PropiedadID == null)
            sCmd.Append(" WHERE PropiedadID IS NULL ");
         else{ 
            // anterior.PropiedadClasificadorID
             sCmd.Append(" WHERE PropiedadID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = anterior.Propiedad.PropiedadID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            // ClasificadorID
            sCmd.Append(" AND ClasificadorID = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
             sqlParam.Value = clasificador.ClasificadorID;
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
            throw new Exception("PropiedadClasificadorUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PropiedadClasificadorUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
