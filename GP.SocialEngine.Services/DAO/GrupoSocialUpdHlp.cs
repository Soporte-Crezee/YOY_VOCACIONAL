using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Actualiza un registro de GrupoSocial en la BD
   /// </summary>
   public class GrupoSocialUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de GrupoSocialUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="grupoSocialUpdHlp">GrupoSocialUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">GrupoSocialUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, GrupoSocial grupoSocial, GrupoSocial anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (grupoSocial == null)
            sError += ", GrupoSocial";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("GrupoSocialUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (grupoSocial.Nombre == null || grupoSocial.Nombre.Trim().Length == 0)
            sError += ", Nombre";
         if (grupoSocial.NumeroMiembros == null)
            sError += ", NumeroMiembros";
         if (anterior.GrupoSocialID == null)
            sError += ", Anterior GrupoSocialID";
         if (anterior.Nombre == null)
            sError += ", Anterior Nombre";
         if (anterior.FechaCreacion == null)
            sError += ", Anterior FechaCracion";
         if (anterior.NumeroMiembros == null)
            sError += ", Anterior NumeroMiembros";
         if (sError.Length > 0)
            throw new Exception("GrupoSocialUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "GrupoSocialUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "GrupoSocialtUpdHl: Hubo un error al conectarse a la base de datos", "GP.SocialEngine.DAO", 
         "GrupoSocialUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE GrupoSocial ");
         if (grupoSocial.Nombre == null)
            sCmd.Append(" SET Nombre = NULL ");
         else{ 
            // grupoSocial.Nombre
            sCmd.Append(" SET Nombre = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = grupoSocial.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (grupoSocial.Descripcion == null)
            sCmd.Append(" ,Descripcion = NULL ");
         else{ 
            // grupoSocial.Descripcion
            sCmd.Append(" ,Descripcion = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = grupoSocial.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         // grupoSocial.NumeroMiembros
         sCmd.Append(" ,NumeroMiembros=@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (grupoSocial.NumeroMiembros == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = grupoSocial.NumeroMiembros;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         if (anterior.GrupoSocialID == null)
            sCmd.Append(" WHERE GrupoSocialID IS NULL ");
         else{ 
            // anterior.GrupoSocialID
            sCmd.Append(" WHERE GrupoSocialID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = anterior.GrupoSocialID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Nombre == null)
            sCmd.Append(" AND Nombre IS NULL ");
         else{ 
            // anterior.Nombre
            sCmd.Append(" AND Nombre = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = anterior.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Descripcion == null)
            sCmd.Append(" AND Descripcion IS NULL ");
         else{ 
            // anterior.Descripcion
            sCmd.Append(" AND Descripcion = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = anterior.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.FechaCreacion == null)
            sCmd.Append(" AND FechaCreacion IS NULL ");
         else{ 
            // anterior.FechaCreacion
            sCmd.Append(" AND FechaCreacion = @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = anterior.FechaCreacion;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.NumeroMiembros == null)
            sCmd.Append(" AND NumeroMiembros IS NULL ");
         else{ 
            // anterior.NumeroMiembros
            sCmd.Append(" AND NumeroMiembros = @dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            sqlParam.Value = anterior.NumeroMiembros;
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
            throw new Exception("GrupoSocialUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("GrupoSocialUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
