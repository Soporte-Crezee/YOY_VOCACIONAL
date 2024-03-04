using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Actualiza un registro de TemaCurso en la BD
   /// </summary>
   internal class TemaCursoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de TemaCurso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="temaCurso">TemaCurso que tiene los datos nuevos</param>
      /// <param name="anterior">TemaCurso que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, TemaCurso temacurso, TemaCurso anterior){
         object myFirm = new object();
         string sError = "";
         if (temacurso == null)
            sError += ", TemaCurso";
         if (anterior == null)
            sError += ", TemaCurso anterior";
         if (sError.Length > 0)
            throw new Exception("TemaCursoUpdHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (temacurso.TemaCursoID == null)
            sError += ", TemaCursoID";
         if (temacurso.Nombre == null)
            sError += ", Nombre";         
         if (temacurso.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (temacurso.Activo == null)
            sError += ", Activo";
         if (anterior.TemaCursoID == null)
            sError += ", TemaCursoID";
         if (anterior.Nombre == null)
            sError += ", Nombre";         
         if (anterior.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (anterior.Activo == null)
            sError += ", Activo";
         if (sError.Length > 0)
            throw new Exception("TemaCursoUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "TemaCursoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TemaCursoUpdHlp: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO", 
         "TemaCursoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE TemaCurso ");
         if (temacurso.Nombre == null)
            sCmd.Append(" SET Nombre = NULL ");
         else{ 
            // temacurso.Nombre
            sCmd.Append(" SET Nombre = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = temacurso.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (temacurso.Descripcion == null)
            sCmd.Append(" ,Descripcion = NULL ");
         else{ 
            // temacurso.Descripcion
            sCmd.Append(" ,Descripcion = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = temacurso.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (temacurso.FechaRegistro == null)
            sCmd.Append(" ,FechaRegistro = NULL ");
         else{ 
            // temacurso.FechaRegistro
            sCmd.Append(" ,FechaRegistro = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = temacurso.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (temacurso.Activo == null)
            sCmd.Append(" ,Activo = NULL ");
         else{ 
            // temacurso.Activo
            sCmd.Append(" ,Activo = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = temacurso.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         sCmd.Append(" WHERE ");
         if (anterior.TemaCursoID != null){
            sCmd.Append(" TemaCursoID = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = anterior.TemaCursoID;
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
            throw new Exception("TemaCursoUpdHlp: Hubo  un Error al Actualizar el Registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("TemaCursoUpdHlp: Hubo  un Error al Actualizar el Registro.");
      }
   } 
}
