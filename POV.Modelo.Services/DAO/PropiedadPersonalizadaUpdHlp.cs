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
   /// Actualiza un registro de PropiedadPersonalizada en la BD
   /// </summary>
   internal class PropiedadPersonalizadaUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de PropiedadPersonalizadaUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="propiedadPersonalizadaUpdHlp">PropiedadPersonalizadaUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">PropiedadPersonalizadaUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, PropiedadPersonalizada propiedad, PropiedadPersonalizada anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (propiedad == null)
            sError += ", PropiedadPersonalizada";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("PropiedadPersonalizadaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.PropiedadID == null)
            sError += ", Anterior PropiedadPersonalizadaID";
         if (sError.Length > 0)
            throw new Exception("PropiedadPersonalizadaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Modelo.DAO", 
         "PropiedadPersonalizadaUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PropiedadPersonalizadaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Modelo.DAO", 
         "PropiedadPersonalizadaUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE PropiedadPersonalizada ");
         if (propiedad.Nombre == null)
            sCmd.Append(" SET Nombre = NULL ");
         else{ 
            // propiedad.Nombre
            sCmd.Append(" SET Nombre = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = propiedad.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (propiedad.Descripcion == null)
            sCmd.Append(" ,Descripcion = NULL ");
         else{ 
            // propiedad.Descripcion
            sCmd.Append(" ,Descripcion = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = propiedad.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (propiedad.EsVisible == null)
             sCmd.Append(" ,EsVisible = NULL ");
         else
         {
             // propiedad.EsVisible
             sCmd.Append(" ,EsVisible = @dbp4ram3 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram3";
             sqlParam.Value = propiedad.EsVisible;
             sqlParam.DbType = DbType.String;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (propiedad.Activo == null)
            sCmd.Append(" ,Activo = NULL ");
         else{ 
            // propiedad.Activo
            sCmd.Append(" ,Activo = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = propiedad.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (propiedad.FechaRegistro == null)
            sCmd.Append(" ,FechaRegistro = NULL ");
         else{ 
            // propiedad.FechaRegistro
            sCmd.Append(" ,FechaRegistro = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = propiedad.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.PropiedadID == null)
            sCmd.Append(" WHERE PropiedadID IS NULL ");
         else{ 
            // anterior.PropiedadPersonalizadaID
            sCmd.Append(" WHERE PropiedadID = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = anterior.PropiedadID;
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
            throw new Exception("PropiedadPersonalizadaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PropiedadPersonalizadaUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
