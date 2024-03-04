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
   /// Actualiza un registro de Modelo Dinámico en la BD
   /// </summary>
   internal class ModeloUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de ModeloDinamico en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="modeloDinamico">ModeloDinamico que tiene los datos nuevos</param>
      /// <param name="anterior">ModeloDinamico que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, AModelo modelo, AModelo anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (modelo == null)
            sError += ", Modelo";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("ModeloDinamicoUpdHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
         if (modelo.Nombre == null || modelo.Nombre.Trim().Length == 0)
            sError += ", Nombre";
         if (modelo.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (modelo.Estatus == null)
            sError += ", Estatus";
         if (modelo.TipoModelo == null)
            sError += ", TipoModelo";         
         if (sError.Length > 0)
            throw new Exception("ModeloDinamicoUpdHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Modelo.DAO", 
         "ModeloDinamicoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ModeloDinamicoUpdHlp: Hubo un error al conectarse a la base de datos", "POV.Modelo.DAO", 
         "ModeloDinamicoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Modelo ");
         if (modelo.TipoModelo == null)
            sCmd.Append(" SET TipoModelo = NULL ");
         else{ 
            // modeloDinamico.TipoModelo
            sCmd.Append(" SET TipoModelo = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = modelo.TipoModelo;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (modelo.Nombre == null)
            sCmd.Append(" ,Nombre = NULL ");
         else{ 
            // modeloDinamico.Nombre
            sCmd.Append(" ,Nombre = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = modelo.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (modelo.Descripcion == null)
            sCmd.Append(" ,Descripcion = NULL ");
         else{ 
            // modeloDinamico.Descripcion
            sCmd.Append(" ,Descripcion = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = modelo.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (modelo.FechaRegistro == null)
            sCmd.Append(" ,FechaRegistro = NULL ");
         else{ 
            // modeloDinamico.FechaRegistro
            sCmd.Append(" ,FechaRegistro = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = modelo.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (modelo.Estatus == null)
            sCmd.Append(" ,Activo = NULL ");
         else{ 
            // modeloDinamico.Estatus
            sCmd.Append(" ,Activo = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = modelo.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (modelo is ModeloDinamico)
         {
             if ((modelo as ModeloDinamico).MetodoCalificacion == null)
                 sCmd.Append(" ,MetodoCalificacion = NULL ");
             else
             {
                 // modeloDinamico.MetodoCalificacion
                 sCmd.Append(" ,MetodoCalificacion = @dbp4ram6 ");
                 sqlParam = sqlCmd.CreateParameter();
                 sqlParam.ParameterName = "dbp4ram6";
                 sqlParam.Value = (modelo as ModeloDinamico).MetodoCalificacion;
                 sqlParam.DbType = DbType.Byte;
                 sqlCmd.Parameters.Add(sqlParam);
             }
         }
         else 
         {
             sCmd.Append(" ,MetodoCalificacion = NULL ");
         }
         
         if (anterior.ModeloID == null)
            sCmd.Append(" WHERE ModeloID IS NULL ");
         else{ 
            // anterior.ModeloID
            sCmd.Append(" WHERE ModeloID = @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = anterior.ModeloID;
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
            throw new Exception("ModeloDinamicoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ModeloDinamicoUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
