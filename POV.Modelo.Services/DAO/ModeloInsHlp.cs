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
   /// Guarda un registro de Modelo Dinámico en la BD
   /// </summary>
   internal class ModeloInsHlp { 
      /// <summary>
      /// Crea un registro de modelo en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="modelo">modelo que desea crear</param>
      public void Action(IDataContext dctx, AModelo modelo){
         object myFirm = new object();
         string sError = String.Empty;
         if (modelo == null)
            sError += ", modelo";
         if (sError.Length > 0)
            throw new Exception("modeloInsHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
         if (modelo.Nombre == null || modelo.Nombre.Trim().Length == 0)
            sError += ", Nombre";
         if (modelo.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (modelo.Estatus == null)
            sError += ", Estatus";
         if (modelo.TipoModelo == null)
            sError += ", TipoModelo";         
         if (sError.Length > 0)
            throw new Exception("modeloInsHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Modelo.DAO", 
         "modeloInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "modeloInsHlp: No se pudo conectar a la base de datos", "POV.Modelo.DAO", 
         "modeloInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO Modelo (TipoModelo, Nombre, Descripcion, FechaRegistro, EsEditable, Activo, MetodoCalificacion) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // modelo.TipoModelo
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (modelo.TipoModelo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = modelo.TipoModelo;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);
         // modelo.Nombre
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (modelo.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = modelo.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // modelo.Descripcion
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (modelo.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = modelo.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // modelo.FechaRegistro
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (modelo.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = modelo.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // modelo.EsEditable
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (modelo.EsEditable == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = modelo.EsEditable;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // modelo.Estatus
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (modelo.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = modelo.Estatus;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // modelo.MetodoCalificacion
         if (modelo is ModeloDinamico)
         {
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if ((modelo as ModeloDinamico).MetodoCalificacion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = (modelo as ModeloDinamico).MetodoCalificacion;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
         }
         else
         {
            sCmd.Append(" ,@dbp4ram7 ");
         }
         
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("modeloInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("modeloInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
